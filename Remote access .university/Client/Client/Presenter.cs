using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ClassLibraryRemotePC;
using System.Drawing;
using System.Diagnostics;


namespace Client
{
    class Presenter
    {
        string CurrentPath;
        MainForm mainForm;
        RemoteScreen remoteScreenForm;
        bool IsFirstOpenRemoteScreenForm = true;
        FileManager fileManagerForm;
        bool IsFirstOpenFileManagerForm = true;
        FileManagerInfo FileManagerInfoClient;
        FileManagerInfo FileManagerInfoServer;

        OperationInterpreter operationInterpreter;
        MessageToUser MessageToUser = new MessageToUser();

        //Сетевые переменные и потоки: 0 - Системные сообщения, 1 - Файловый менеджер, 2 - передача файлов, 3 - трансляция экрана, 4 - управление, 5 - текстовый чат
        List<TcpClient> TcpClients = new List<TcpClient>();
        List<NetworkStream> NetworkStreams = new List<NetworkStream>();
        List<Thread> ReciveData = new List<Thread>();

        List<string> ReciveStrings = new List<string>();

        string ServerIp;
        int ServerPort;
        short NumClientOnServer;

        const short ClientNum = -1;
        bool IsServerConnected = false;
        string RemotePC_Ip;
        string ThisPC_Ip;

        TimerCallback RecivingSpeedTimer;
        System.Threading.Timer timer;
        int num = 0;
        int Speed = 0;
        static StreamWriter swSpeed;

        TimerCallback timerFPS;
        System.Threading.Timer timer1;
        int counter_received_frames = 0;
        static StreamWriter swFPS;

        private static System.Object lockThis = new System.Object();

        public Presenter(MainForm _mainForm)
        {
            mainForm = _mainForm;
            operationInterpreter = new OperationInterpreter(false);

            List<string> Languages = new List<string>();
            Languages.Add("English");
            Languages.Add("Українська");
            Languages.Add("Русский");
            mainForm.SetComboBox_Language(Languages);


            operationInterpreter.Disconnecting += OperationInterpreter_Disconnecting;
            operationInterpreter.InitiateAdditionalConnection += OperationInterpreter_InitiateAdditionalConnection;

            for (int i = 0; i < 6; i++)
            {
                TcpClients.Add(null);
                NetworkStreams.Add(null);
                ReciveData.Add(null);
            }

            mainForm.MainFormClosed += MainForm_MainFormClosed;
            mainForm.OpenFileManeger += MainForm_OpenFileManeger;
            mainForm.OpenRemoteScreen += MainForm_OpenRemoteScreen;
            mainForm.SendTextMessage += MainForm_SendTextMessage;
            mainForm.Connect += MainForm_Connect;
            mainForm.RestartRemotePC += MainForm_RestartRemotePC;

            RecivingSpeedTimer = new TimerCallback(CountSpeed);
            if (File.Exists(CurrentPath + "Count Speed.txt"))
            {
                File.Delete(CurrentPath + "Count Speed.txt");
            }

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            CurrentPath = directory.ToString();
            CurrentPath = CurrentPath + "\\";

            timerFPS = new TimerCallback(CountFPS);
            if (File.Exists(CurrentPath + "Count FPS.txt"))
            {
                File.Delete(CurrentPath + "Count FPS.txt");
            }



            //подгрузку из файла значениями по умолчанию
        }

        public void WriteStringInFile(string StringToWrite)
        {
            lock (lockThis)
            {
                string Time = DateTime.Now.ToLongTimeString();
                using (StreamWriter streamWriter = File.AppendText(CurrentPath + "Client_log.txt"))
                {
                    streamWriter.WriteLine(Time + " ::"  + StringToWrite);
                }
            }
        }
        private void CountFPS(object obj)
        {
            string Time = DateTime.Now.ToLongTimeString();
            using (swFPS = File.AppendText(CurrentPath + "Count FPS.txt"))
            {
                swFPS.WriteLine(Time + " ::" +Convert.ToString(counter_received_frames));
            }
            counter_received_frames = 0;
        }
        private void CountSpeed(object obj)
        {
            string Time = DateTime.Now.ToLongTimeString();
            string Res = FileSizeAndMeasure((long)Speed);
            Speed = 0;
            Res = Res + "/s";
            fileManagerForm.Set_ReceivingSpeed(Res);
            using ( swSpeed = File.AppendText(CurrentPath + "Count Speed.txt"))
            {
                swSpeed.WriteLine(Time + " ::" + Res);
            }
        }
        private bool StopAllClientThreads()
        {
            bool res;
            try
            {
                foreach (Thread thread in ReciveData)
                {
                    if (thread.IsAlive == true)
                    {
                        thread.Abort();
                    }
                }
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }
        string FileSizeAndMeasure(long size)
        {
            string res = "0 Byte";
            if (size <= 1000)
            {
                res = size + " Byte";
            }
            else if (size > 1000)
            {
                int count = 0;
                do
                {
                    size = size / 1000;
                    count++;
                }
                while (size > 1000);
                if (count == 1)
                    res = size + " KB";
                if (count == 2)
                    res = size + " MB";
                if (count == 3)
                    res = size + " GB";
                if (count == 4)
                    res = size + " TB";
            }
            return res;
        }

        
        #region Обработка событий Интерпретатора КОП
        private void OperationInterpreter_InitiateAdditionalConnection(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Initiate Additional Connection. Client " + operationCod.NumberPC, true);
            AdditionalConnect(ServerPort, ServerIp, operationCod);
            SendOperationCode(operationCod, NetworkStreams[0]);
        }
        private void OperationInterpreter_Disconnecting(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            mainForm.TextMessenger_AllMessenges("System", "Disconnecting. Client " + operationCod.NumberPC, true);
            SendOperationCode(operationCod, NetworkStreams[0]);
            //for (int i = 0; i < 6; i++)
            //{
            //    try
            //    {
            //        Disconnect(i);
            //    }
            //    catch
            //    {
            //        MessageToUser.ShowError("Disconnect " + i + " connection failed!", "Client");
            //    }
            //}
        }//Переписать!!!
        #endregion

        #region Обработка событий Main Формы
        private void MainForm_Connect(object sender, EventArgs e)
        {
            if (mainForm.GetConnectButtonText() == "Connect")
            {
                string Ip = mainForm.GetIp();
                string PortStr = mainForm.GetPort();
                int count = 0;
                bool IpOk = false;
                bool PortOk = false;
                int Port = 0;
                string Password = mainForm.GetPassword();
                bool PasswordOK = false;

                if (Ip == "")
                {
                    MessageToUser.ShowError("\"Ip\" field empty!", "Client, IP Error");
                }
                else
                {
                    string[] parts = Ip.Split('.');
                    foreach (string part in parts)
                    {
                        try
                        {
                            int num = Convert.ToInt32(part);
                            if (num >= 0 && num <= 255)
                            {
                                count++;
                            }
                        }
                        catch
                        {
                            MessageToUser.ShowError("Ip address is not an Ip address! Ip address must be of the form \"0.0.0.0\" and can range from 0 to 255.", "Client, IP Error");
                        }
                    }
                    if (count == 4)
                    {
                        IpOk = true;
                    }
                    else
                    {
                        MessageToUser.ShowError("Ip address is not an Ip address! Ip address must be of the form \"0.0.0.0\" and can range from 0 to 255.", "Client, IP Error");
                    }
                }
                if (PortStr == "")
                {
                    MessageToUser.ShowError("\"Port\" field empty!", "Client, Port Error");
                }
                else
                {
                    try
                    {
                        Port = Convert.ToInt32(PortStr);
                        PortOk = true;
                    }
                    catch
                    {
                        MessageToUser.ShowError("\"Port\" field must contain only numbers!", "Client, Port Error");
                    }
                    if (PortOk == true)
                    {
                        if (Port < 49152 || Port > 65535)
                        {
                            PortOk = false;
                            MessageToUser.ShowError("\"Port\" field must contain number in the range from 49152 to 65535!", "Client, Port Error");
                        }
                    }
                }
                if (Password == "")
                {
                    MessageToUser.ShowError("\"Password\" field empty!", "Client, Password Error");
                }
                else
                {
                    if (Encoding.Unicode.GetBytes(Password).Length <= 100)
                    {
                        PasswordOK = true;
                    }
                    else
                    {
                        MessageToUser.ShowError("\"Password\" field must be between 1 and 50 characters!", "Client, Password Error");
                    }
                }
                if (IpOk == true && PortOk == true && PasswordOK == true)
                {
                    ServerIp = Ip;
                    ServerPort = Port;
                    FirstConnect(ServerPort, ServerIp, Password);
                    mainForm.SetConnectButtonText("Disconnect");
                }
            }
            else if(mainForm.GetConnectButtonText() == "Disconnect")
            {
                //operationInterpreter.CodeDisconnectingRequest()
                StopAllClientThreads();
                for (int i = 0; i < 6; i++)
                {
                    Disconnect(i);
                }
                mainForm.SetConnectButtonText("Connect");
            }
        }
        private void MainForm_MainFormClosed(object sender, EventArgs e)
        {
            StopAllClientThreads();
        }
        private void MainForm_SendTextMessage(object sender, EventArgs e)
        {
            string messege = mainForm.Get_TextMessenge();
            if (messege != "")
            {
                SendTextMessage(messege, NetworkStreams[5]);
                string NameDate = DateTime.Now.ToLongTimeString() + " - " + "Client (" + ThisPC_Ip + "): ";
                mainForm.TextMessenger_AllMessenges(NameDate, messege, true);
            }
        }
        private void MainForm_OpenRemoteScreen(object sender, EventArgs e)
        {
            remoteScreenForm = new RemoteScreen();
            remoteScreenForm.Text = remoteScreenForm.Text + RemotePC_Ip;
            remoteScreenForm.RemoteScreenFormClosed += RemoteScreenForm_RemoteScreenFormClosed;
            remoteScreenForm.Show();
            if (IsFirstOpenRemoteScreenForm == true)
            {
                OperationCod StartRemoteScreen = operationInterpreter.CodeWork_RemoteScreen_Request(NumClientOnServer);
                SendOperationCode(StartRemoteScreen, NetworkStreams[0]);
                IsFirstOpenRemoteScreenForm = false;
            }
            else
            {
                if (remoteScreenForm.IsHandleCreated == true)
                {
                    OperationCod StartRemoteScreen = operationInterpreter.Code_StartRemoteScreen(NumClientOnServer);
                    SendOperationCode(StartRemoteScreen, NetworkStreams[0]);
                    ReciveData[3] = new Thread(ReciveScreenImage);
                    ReciveData[3].Start(NetworkStreams[3]);
                }
            }
        }
        private void MainForm_OpenFileManeger(object sender, EventArgs e)
        {
            OperationCod CodFileManagerRequest = operationInterpreter.CodeWorkWithTheFileManagerRequest(NumClientOnServer, -1);
            SendOperationCode(CodFileManagerRequest, NetworkStreams[0]);

            fileManagerForm = new FileManager();
            fileManagerForm.FileManagerFormLoad += FileManagerForm_FileManagerFormLoad;
            fileManagerForm.MouseDoubleClick_ThisPC += FileManagerForm_MouseDoubleClick_ThisPC;
            fileManagerForm.MouseDoubleClick_RemotePC += FileManagerForm_MouseDoubleClick_RemotePC;
            fileManagerForm.Back_ThisPC += FileManagerForm_Back_ThisPC;
            fileManagerForm.Back_RemotePC += FileManagerForm_Back_RemotePC;
            fileManagerForm.SelectedLD_ThisPC += FileManagerForm_SelectedLD_ThisPC;
            fileManagerForm.SelectedLD_RemotePC += FileManagerForm_SelectedLD_RemotePC;

            fileManagerForm.PropertiesFmObjClick_ThisPC += FileManagerForm_PropertiesFmObjClick_ThisPC;
            fileManagerForm.PropertiesFmObjClick_RemotePC += FileManagerForm_PropertiesFmObjClick_RemotePC;
            fileManagerForm.FmLd_RightClick_ThisPC += FileManagerForm_FmLd_RightClick_ThisPC;
            fileManagerForm.FmLd_RightClick_RemotePC += FileManagerForm_FmLd_RightClick_RemotePC;

            fileManagerForm.FmDelete_RightClick_RemotePC += FileManagerForm_FmDelete_RightClick_RemotePC;
            fileManagerForm.FmDelete_RightClick_ThisPC += FileManagerForm_FmDelete_RightClick_ThisPC;

            fileManagerForm.SendFile_toRemotePC += FileManagerForm_SendFile_toRemotePC;
            fileManagerForm.SendFile_toThisPC += FileManagerForm_SendFile_toThisPC;
            fileManagerForm.Show();
        }
        private void MainForm_RestartRemotePC(object sender, EventArgs e)
        {
            var shutdown = new ProcessStartInfo("shutdown", @"-r -t 60 -f -m \\" + RemotePC_Ip);
            shutdown.CreateNoWindow = true;
            shutdown.UseShellExecute = false;
            Process.Start(shutdown);
            mainForm.Messege_Status("Server (" + RemotePC_Ip + ") reboot will probably be done in 60 seconds.", 3);
            WriteStringInFile("Server (" + RemotePC_Ip + ") reboot will probably be done in 60 seconds.");
        }

        #endregion

        #region Обработка событий File Manager Формы
        private void FileManagerForm_Back_RemotePC(object sender, EventArgs e)
        {
            string path = GetPreviousPath(FileManagerInfoServer.ThisDirPath);
            OperationCod CodFileManagerUserDir = operationInterpreter.Code_InformationAboutLogicalDrives_AndСustomDirectoryObjects(NumClientOnServer, path);
            SendOperationCode(CodFileManagerUserDir, NetworkStreams[0]);
        }
        private void FileManagerForm_Back_ThisPC(object sender, EventArgs e)
        {
            FileManagerInfoClient = new FileManagerInfo(GetPreviousPath(FileManagerInfoClient.ThisDirPath));
            ShowFM(FileManagerInfoClient, true);
        }
        private void FileManagerForm_SendFile_toThisPC(object sender, EventArgs e)
        {
            int FileIndexRemotePC = (int)sender;

            string ReadPath = FileManagerInfoServer.fileSystemObjectsInfo[FileIndexRemotePC].FullName;
            string WritePath = FileManagerInfoClient.fileSystemObjectsInfo[0].FullName;
            int index = FileManagerInfoClient.fileSystemObjectsInfo[0].FullName.LastIndexOf('\\');
            WritePath = WritePath.Substring(0, index);
            OperationCod SendFileCod = operationInterpreter.Code_SendFile(NumClientOnServer, ReadPath, WritePath);
            SendOperationCode(SendFileCod, NetworkStreams[0]);
        }
        private void FileManagerForm_SendFile_toRemotePC(object sender, EventArgs e)
        {
            int FileIndexThisPC = (int)sender;
            string WritePath = FileManagerInfoServer.ThisDirPath;
            string ReadPath = FileManagerInfoClient.fileSystemObjectsInfo[FileIndexThisPC].FullName;
            SendFile(NetworkStreams[2], ReadPath, WritePath);
        }
        private void FileManagerForm_MouseDoubleClick_RemotePC(object sender, EventArgs e)
        {
            int selectNum = (int)sender;
            string path = FileManagerInfoServer.fileSystemObjectsInfo[selectNum].FullName;
            if ((FileManagerInfoServer.Attributes[selectNum] & FileAttributes.Directory) == FileAttributes.Directory)
            {
                OperationCod CodFileManagerUserDir = operationInterpreter.Code_InformationAboutLogicalDrives_AndСustomDirectoryObjects(NumClientOnServer, path);
                SendOperationCode(CodFileManagerUserDir, NetworkStreams[0]);
            }
            else
            {
                OperationCod CodFileManagerUserDir = operationInterpreter.LaunchFile_Code(NumClientOnServer, path);
                SendOperationCode(CodFileManagerUserDir, NetworkStreams[0]);
            }
        }
        private void FileManagerForm_MouseDoubleClick_ThisPC(object sender, EventArgs e)
        {
            int selectNum = (int)sender;
            string path = FileManagerInfoClient.fileSystemObjectsInfo[selectNum].FullName;
            if ((FileManagerInfoClient.fileSystemObjectsInfo[selectNum].Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                FileManagerInfoClient = new FileManagerInfo(path);
                ShowFM(FileManagerInfoClient, true);
            }
            else
            {
                // System.Diagnostics.Process.Start(path);
                Process p = new Process();
                ProcessStartInfo pi = new ProcessStartInfo();
                pi.UseShellExecute = true;
                pi.FileName = path;
                p.StartInfo = pi;

                try
                {
                    p.Start();
                    WriteStringInFile("Launch file (" + path + ") Client: " + ThisPC_Ip);
                    mainForm.Messege_Status("Launch file (" + path + ") Client: " + ThisPC_Ip, 3);
                    fileManagerForm.Messege_Status("Launch file (" + path + ").", 1);
                }
                catch (Exception Ex)
                {
                    WriteStringInFile("Launch file (" + path + ") failed. Client: " + ThisPC_Ip);
                    mainForm.Messege_Status("Launch file (" + path + ") failed. Client: " + ThisPC_Ip, 2);
                    fileManagerForm.Messege_Status("Launch file (" + path + ") failed.", 2);
                }
            }
        }
        private void FileManagerForm_FileManagerFormLoad(object sender, EventArgs e)
        {
            fileManagerForm.Set_RemotePC_Name(RemotePC_Ip);
            fileManagerForm.Set_ThisPC_Name(ThisPC_Ip);
            FileManagerInfoClient = new FileManagerInfo();
            ShowFM(FileManagerInfoClient, true);
        }
        private void FileManagerForm_FmDelete_RightClick_ThisPC(object sender, EventArgs e)
        {
            int FileIndexThisPC = (int)sender;
            string path = FileManagerInfoClient.fileSystemObjectsInfo[FileIndexThisPC].FullName;
            int res = DeleteFile(path);
            if (res == 1)
            {
                WriteStringInFile("Deleting file (" + path + ") Client: " + ThisPC_Ip);
                mainForm.Messege_Status("Deleting file (" + path + ") Client: " + ThisPC_Ip, 3);
                fileManagerForm.Messege_Status("Deleting file.", 1);
                FileManagerInfoClient = new FileManagerInfo(FileManagerInfoClient.ThisDirPath);
                ShowFM(FileManagerInfoClient, true);

            }
            else if (res == 0)
            {
                WriteStringInFile("File (" + path + ") doesn't exist. Client: " + ThisPC_Ip);
                mainForm.Messege_Status("File (" + path + ") doesn't exist. Client: " + ThisPC_Ip, 2);
                fileManagerForm.Messege_Status("File (" + path + ") doesn't exist.", 2);
            }
            else if (res == -1)
            {
                WriteStringInFile("Delete file (" + path + ") failed. Client: " + ThisPC_Ip);
                mainForm.Messege_Status("Delete file (" + path + ") failed. Client: " + ThisPC_Ip, 2);
                fileManagerForm.Messege_Status("Delete file (" + path + ") failed.", 2);
            }
        }
        private void FileManagerForm_FmDelete_RightClick_RemotePC(object sender, EventArgs e)
        {
            int FileIndexRemotePC = (int)sender;
            string path = FileManagerInfoServer.fileSystemObjectsInfo[FileIndexRemotePC].FullName;
            OperationCod DeleteFileCod = operationInterpreter.DeleteFile_Code(NumClientOnServer, path);
            SendOperationCode(DeleteFileCod, NetworkStreams[0]);
            path = FileManagerInfoServer.ThisDirPath;
            OperationCod CodFileManagerUserDir = operationInterpreter.Code_InformationAboutLogicalDrives_AndСustomDirectoryObjects(NumClientOnServer, path);
            SendOperationCode(CodFileManagerUserDir, NetworkStreams[0]);
        }
        private void FileManagerForm_FmLd_RightClick_RemotePC(object sender, EventArgs e)
        {
            string info = "";
            string LD = (string)sender;
            string SizeMeasure = "";
            for (int i = 0; i < FileManagerInfoServer.drivesInfo.Count; i++)
            {
                if (FileManagerInfoServer.drivesInfo[i].Name == LD)
                {
                    info = info + "Name: " + FileManagerInfoServer.drivesInfo[i].Name + System.Environment.NewLine;
                    info = info + "Drive Format: " + FileManagerInfoServer.drivesInfo[i].DriveFormat + System.Environment.NewLine;

                    SizeMeasure = FileSizeAndMeasure(FileManagerInfoServer.drivesInfo[i].AvailableFreeSpace);
                    info = info + "Available Free Space: " + SizeMeasure + " (" + FileManagerInfoServer.drivesInfo[i].AvailableFreeSpace + "byte)" + System.Environment.NewLine;
                    SizeMeasure = FileSizeAndMeasure(FileManagerInfoServer.drivesInfo[i].TotalSize);
                    info = info + "Total Size: " + SizeMeasure + " (" + FileManagerInfoServer.drivesInfo[i].TotalSize + "byte)" + System.Environment.NewLine;

                    fileManagerForm.ShowInfo(info, "Remote PC: " + RemotePC_Ip);
                }
            }
        }
        private void FileManagerForm_FmLd_RightClick_ThisPC(object sender, EventArgs e)
        {
            string info = "";
            string LD = (string)sender;
            string SizeMeasure = "";
            for (int i = 0; i < FileManagerInfoClient.drivesInfo.Count; i++)
            {
                if (FileManagerInfoClient.drivesInfo[i].Name == LD)
                {
                    info = info + "Name: " + FileManagerInfoClient.drivesInfo[i].Name + System.Environment.NewLine;
                    info = info + "Drive Format: " + FileManagerInfoClient.drivesInfo[i].DriveFormat + System.Environment.NewLine;

                    SizeMeasure = FileSizeAndMeasure(FileManagerInfoClient.drivesInfo[i].AvailableFreeSpace);
                    info = info + "Available Free Space: " + SizeMeasure + " (" + FileManagerInfoClient.drivesInfo[i].AvailableFreeSpace + "byte)" + System.Environment.NewLine;
                    SizeMeasure = FileSizeAndMeasure(FileManagerInfoClient.drivesInfo[i].TotalSize);
                    info = info + "Total Size: " + SizeMeasure + " (" + FileManagerInfoClient.drivesInfo[i].TotalSize + "byte)" + System.Environment.NewLine;

                    fileManagerForm.ShowInfo(info, "This PC: " + ThisPC_Ip);
                }
            }
        }
        private void FileManagerForm_PropertiesFmObjClick_RemotePC(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void FileManagerForm_PropertiesFmObjClick_ThisPC(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }
        private void FileManagerForm_SelectedLD_RemotePC(object sender, EventArgs e)
        {
            int index = (int)sender;
            string path = FileManagerInfoServer.drivesInfo[index].RootDirectory.FullName;
            OperationCod CodFileManagerUserDir = operationInterpreter.Code_InformationAboutLogicalDrives_AndСustomDirectoryObjects(NumClientOnServer, path);
            SendOperationCode(CodFileManagerUserDir, NetworkStreams[0]);

        }
        private void FileManagerForm_SelectedLD_ThisPC(object sender, EventArgs e)
        {
            int index = (int)sender;
            string path = FileManagerInfoClient.drivesInfo[index].RootDirectory.FullName;
            FileManagerInfoClient = new FileManagerInfo(path);
            ShowFM(FileManagerInfoClient, true);
        }
        #endregion

        #region Обработка событий Remote Screen Формы
        private void RemoteScreenForm_RemoteScreenFormClosed(object sender, EventArgs e)
        {
            timer1.Change(Timeout.Infinite, 1000);
            ReciveData[3].Abort();
            OperationCod StopRemoteScreen = operationInterpreter.Code_StopRemoteScreen(-1);
            SendOperationCode(StopRemoteScreen, NetworkStreams[0]);
        }
        #endregion

        #region Подключение отключение
        void FirstConnect(int _port, string _ip, string _password)
        {
            if (IsServerConnected == false)
            {
                try
                {
                    TcpClient _tcpClient = new TcpClient();
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(_ip, _port);

                    byte[] data1 = Encoding.Unicode.GetBytes(_password);
                    _tcpClient.GetStream().Write(data1, 0, data1.Length);

                    StringBuilder ClientPassword = new StringBuilder();
                    byte[] data = new byte[100];
                    int bytes = 0;
                    bytes = _tcpClient.GetStream().Read(data, 0, data.Length);
                    ClientPassword.Append(Encoding.Unicode.GetString(data, 0, bytes));

                    if (ClientPassword.ToString() == "Good!")
                    {

                        byte[] ClientNumOnServ = new byte[2];

                        bytes = _tcpClient.GetStream().Read(ClientNumOnServ, 0, ClientNumOnServ.Length);
                        NumClientOnServer = BitConverter.ToInt16(ClientNumOnServ, 0);

                        RemotePC_Ip = IPAddress.Parse(((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString()).ToString();

                        StringBuilder ClientIP = new StringBuilder();
                        data = new byte[100];
                        bytes = 0;
                        bytes = _tcpClient.GetStream().Read(data, 0, data.Length);
                        ClientIP.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        ThisPC_Ip = ClientIP.ToString();

                        TcpClients[0] = _tcpClient;
                        NetworkStreams[0] = _tcpClient.GetStream();
                        ReciveData[0] = new Thread(ReciveOperationCode);
                        ReciveData[0].Start(NetworkStreams[0]);
                        IsServerConnected = true;
                        mainForm.Messege_Status("Сonnection to " + RemotePC_Ip + " server established!", 1);
                        WriteStringInFile("Сonnection to " + RemotePC_Ip + " server established!");
                        //MessageToUser.ShowInformation("Сonnection to " + RemotePC_Ip + " server established!", "Client");
                    }
                    else
                    {
                        MessageToUser.ShowError("Сonnection to server failed! Wrong password!", "Client Сonnection Error");
                    }
                }
                catch
                {
                    MessageToUser.ShowError("Сonnection to server failed! Make sure that the values of the port number and Ip addresses match those of the server.", "Client Сonnection Error");
                }
            }//первое подключение Дописать идентификацию на сервере и получить ответ о подключении.
        }
        void AdditionalConnect(int _port, string _ip, OperationCod _operationCod)
        {
            if (IsServerConnected == true)
            {
                try
                {
                    TcpClient _tcpClient = new TcpClient();
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(_ip, _port);

                    if (_operationCod.OperationTypes[1] == 1 && _operationCod.ChainPartNumber == 0)
                    {
                        TcpClients[1] = _tcpClient;
                        NetworkStreams[1] = _tcpClient.GetStream();
                        ReciveData[1] = new Thread(ReceiveFSInfo);
                        ReciveData[1].Start(NetworkStreams[1]);
                    }//первое доп. подключение файлового менеджера. Инфа о ФС.
                    else if(_operationCod.OperationTypes[1] == 1 && _operationCod.ChainPartNumber == 1)
                    {
                        TcpClients[2] = _tcpClient;
                        NetworkStreams[2] = _tcpClient.GetStream();
                        ReciveData[2] = new Thread(ReciveFile);
                        ReciveData[2].Start(NetworkStreams[2]);
                    }//второе доп. подключение файлового менеджера. Передача файлов.
                    else if (_operationCod.OperationTypes[1] == 2 && _operationCod.ChainPartNumber == 0)
                    {
                        TcpClients[3] = _tcpClient;
                        NetworkStreams[3] = _tcpClient.GetStream();
                        ReciveData[3] = new Thread(ReciveScreenImage);
                        ReciveData[3].Start(NetworkStreams[3]);
                    }//доп. подключение трансляции экрана. Дописать метод прослушивания трансляции!!!
                    else if (_operationCod.OperationTypes[1] == 5 && _operationCod.ChainPartNumber == 0)
                    {
                        TcpClients[5] = _tcpClient;
                        NetworkStreams[5] = _tcpClient.GetStream();
                        ReciveData[5] = new Thread(ReceiveTextMessage);
                        ReciveData[5].Start(NetworkStreams[5]);
                    }//Текстовый чат

                }
                catch
                {
                    MessageToUser.ShowError("Additional connection to server failed!", "Client - connection");
                }
            }//дополнительное подключение ДОПИСАТЬ КОГДА ДОБАВИТСЯ ТРАНСЛЯЦИЯ, УПРАВЛЕНИЕ, ТЕКСТОВЫЙ ЧАТ
        }
        void Disconnect(int num)
        {
            if (NetworkStreams[num] != null)
                NetworkStreams[num].Close();//отключение потока
            if (TcpClients[num] != null)
                TcpClients[num].Close();//отключение клиента
            if (ReciveData[num] != null)
                ReciveData[num].Abort();
            //Environment.Exit(0); //завершение процесса
        }
        #endregion

        #region Текстовые сообщения 
        protected internal void ReceiveTextMessage(object _networkStream)
        {
            NetworkStream networkStream = (NetworkStream)_networkStream;
            //MessageToUser.ShowInformation("Прослушивание текстовых сообщений", "Client");
            while (true)
            {
                try
                {
                    byte[] data = new byte[512];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = networkStream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (networkStream.DataAvailable);

                    string NameDate = DateTime.Now.ToLongTimeString() + " - " + "Server (" + RemotePC_Ip + "): ";
                    mainForm.TextMessenger_AllMessenges(NameDate, builder.ToString(), false);
                }
                catch
                {
                    MessageToUser.ShowError("Ошибка прослушивание текстовых сообщений!", "Client Error!");
                    Disconnect(5);
                }
            }
        }
        void SendTextMessage(string message, NetworkStream _networkStream)
        {
            byte[] data1 = Encoding.Unicode.GetBytes(message);
            _networkStream.Write(data1, 0, data1.Length);
        }
        #endregion

        #region Трансляция экрана
        private void ReciveScreenImage(object networkStream)
        {
            //MessageToUser.ShowInformation("Получение трансляции экрана запущенно!", "Client");
            NetworkStream _NetworkStream = (NetworkStream)networkStream;
            int bytes;
             int bufferSize = 8192;
            int BytesToRead;

            timer1 = new System.Threading.Timer(timerFPS, num, 0, 1000);

            while (true)
            {
                byte[] ImageSize = new byte[4];
                bytes = _NetworkStream.Read(ImageSize, 0, ImageSize.Length);
                BytesToRead = BitConverter.ToInt32(ImageSize, 0);
                bytes = 0;
                bufferSize = BytesToRead;
                NetImage Image;
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    do
                    {
                        int received = _NetworkStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, received);
                        bytes += received;
                    }
                    while (!(BytesToRead == bytes));
                    Image = new NetImage(memStream.ToArray());
                    remoteScreenForm.SG_pictureBox_Screen = Image.image;
                    counter_received_frames++;
                }
            }
        }
        private void SendScreenImage(NetImage _image, NetworkStream _networkStream)
        {
            byte[] SendImage = _image.ToArray();
            byte[] ImageSize = BitConverter.GetBytes(SendImage.Length);
            _networkStream.Write(ImageSize, 0, 4);
            _networkStream.Write(SendImage, 0, SendImage.Length);
        }
        #endregion

        #region Системные сообщения
        private void ReciveOperationCode(object networkStream)
        {
            //MessageToUser.ShowInformation("Получение системных сообщений от сервера " + ServerIp + " запущенно!", "Client");
            NetworkStream _NetworkStream = (NetworkStream)networkStream;
            int bytes;
            const int bufferSize = 8192;
            int BytesToRead;

            while (true)
            {
                byte[] OpCodeSize = new byte[4];
                bytes = _NetworkStream.Read(OpCodeSize, 0, OpCodeSize.Length);
                BytesToRead = BitConverter.ToInt32(OpCodeSize, 0);
                bytes = 0;

                OperationCod OpCod;
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    do
                    {
                        int received = _NetworkStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, received);
                        bytes += received;
                    }
                    while (!(BytesToRead == bytes));
                    OpCod = new OperationCod(memStream.ToArray());
                    OpCod.NumberPC = ClientNum;
                    operationInterpreter.AddOnExecutionOperationCode(OpCod);
                }
            }
        }
        private void SendOperationCode(OperationCod operationCod, NetworkStream _networkStream)
        {
            operationCod.NumberPC = NumClientOnServer;
            byte[] SendOpCode = operationCod.ToArray();
            byte[] OpCodeSize = BitConverter.GetBytes(SendOpCode.Length);
            _networkStream.Write(OpCodeSize, 0, 4);
            _networkStream.Write(SendOpCode, 0, SendOpCode.Length);
        }
        #endregion

        #region Файловая система
        public string GetPreviousPath(string path)
        {
            int index = path.LastIndexOf('\\');
            string PeviousPath = path.Substring(0, index);
            if (PeviousPath.Length == 2)
            {
                PeviousPath = PeviousPath + "\\";
            }
            return PeviousPath;
        }
        public void ShowFM (FileManagerInfo FM_Info, bool IsThisPC)
        {
            List<string> LD_Names = new List<string>();
            for (int i = 0; i < FM_Info.drivesInfo.Count(); i++)
            {
                LD_Names.Add(FM_Info.drivesInfo[i].Name);
            }
            if (IsThisPC == true)
            {
                fileManagerForm.SetComboBox_LD_ThisPC(LD_Names);
            }
            else
            {
                fileManagerForm.SetComboBox_LD_RemotePC(LD_Names);
            }

            List<string> Obj_Names = new List<string>();
            List<string> Obj_DatesOfChange = new List<string>();
            List<string> Obj_Types = new List<string>();
            List<string> Obj_Sizes = new List<string>();

            for (int i = 0; i < FM_Info.fileSystemObjectsInfo.Count(); i++)
            {
                Obj_Names.Add(FM_Info.fileSystemObjectsInfo[i].Name);

                if (IsThisPC == true)
                {
                    Obj_DatesOfChange.Add(FM_Info.fileSystemObjectsInfo[i].LastWriteTime.ToString());
                    if ((FM_Info.fileSystemObjectsInfo[i].Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        Obj_Types.Add("Directory");
                        Obj_Sizes.Add("");
                    }
                    else
                    {
                        Obj_Types.Add("File");
                        try
                        {
                            FileInfo fileInfo = new FileInfo(FM_Info.fileSystemObjectsInfo[i].FullName);
                            long size = fileInfo.Length;
                            Obj_Sizes.Add(FileSizeAndMeasure(size));
                        }
                        catch
                        {
                            Obj_Sizes.Add("");
                        }
                    }
                }
                else
                {
                    Obj_DatesOfChange.Add(FM_Info.LastWriteTime[i].ToString());
                    if ((FM_Info.Attributes[i] & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        Obj_Types.Add("Directory");
                        Obj_Sizes.Add("");
                    }
                    else
                    {
                        Obj_Types.Add("File");
                        if (FM_Info.LengthObj[i] == -1)
                        {
                            Obj_Sizes.Add("");
                        }
                        else
                        {
                            Obj_Sizes.Add(FileSizeAndMeasure(FM_Info.LengthObj[i]));
                        }
                       
                    }
                }
            }
            if (IsThisPC == true)
            {
                fileManagerForm.Set_listView_ThisPC(Obj_Names, Obj_DatesOfChange, Obj_Types, Obj_Sizes);
                fileManagerForm.Set_textBox_Path_ThisPC(FM_Info.ThisDirPath);
            }
            else
            {
                fileManagerForm.Set_listView_RemotePC(Obj_Names, Obj_DatesOfChange, Obj_Types, Obj_Sizes);
                fileManagerForm.Set_textBox_Path_RemotePC(FM_Info.ThisDirPath);
            }
        }
        protected internal void ReceiveFSInfo(object _networkStream)
        {
            //MessageToUser.ShowInformation("Получение сообщений с инфой о ФС от сервера " + ServerIp + " запущенно!", "Client");
            NetworkStream networkStream = (NetworkStream)_networkStream;
            int bytes;
            const int bufferSize = 8192;
            int BytesToRead;

            while (true)
            {
                byte[] FS_Info_Size = new byte[4];
                bytes = networkStream.Read(FS_Info_Size, 0, FS_Info_Size.Length);
                BytesToRead = BitConverter.ToInt32(FS_Info_Size, 0);
                bytes = 0;

                FileManagerInfo FS_Info;
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    do
                    {
                        int received = networkStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, received);
                        bytes += received;
                    }
                    while (!(BytesToRead == bytes));
                    FS_Info = new FileManagerInfo(memStream.ToArray());

                    FileManagerInfoServer = FS_Info;
                    ShowFM(FileManagerInfoServer, false);
                }
            }
        }
        private void SendFSInfo(FileManagerInfo FS_Info, NetworkStream _networkStream)
        {
            byte[] Send_FS_Info = FS_Info.ToArray();
            byte[] FS_Info_Size = BitConverter.GetBytes(Send_FS_Info.Length);
            _networkStream.Write(FS_Info_Size, 0, 4);
            _networkStream.Write(Send_FS_Info, 0, Send_FS_Info.Length);
        }
        private int DeleteFile(string path)
        {
            int res = 1;
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    res = -1;
                }
            }
            else
            {
                res = 0;
            }
            return res;
        }
        #endregion

        #region Передача файлов
        public void SendFile(NetworkStream _networkStream, string _SendFilePath, string _WriteFilePath)
        {
            WriteStringInFile("Send file (" + _SendFilePath + ") to " + RemotePC_Ip + " server");
            mainForm.Messege_Status("Send file (" + _SendFilePath + ") to " + RemotePC_Ip + " server", 1);
            fileManagerForm.Messege_Status("Sending file...", 1);

            using (FileStream stream = new FileStream(_SendFilePath, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[stream.Length];
                int length = stream.Read(data, 0, data.Length);
                NetFile file = new NetFile();
                file.FileName = Path.GetFileName(stream.Name);
                file.Data = data;

                byte[] SendFile = file.ToArray();
                byte[] FileSize = BitConverter.GetBytes(SendFile.Length);
                byte[] PathToWrite = Encoding.UTF8.GetBytes(_WriteFilePath);
                byte[] PathSize = BitConverter.GetBytes(PathToWrite.Length);

                _networkStream.Write(PathSize, 0, 4);
                _networkStream.Write(PathToWrite, 0, PathToWrite.Length);
                _networkStream.Write(FileSize, 0, 4);
                _networkStream.Write(SendFile, 0, SendFile.Length);
            }
        }
        void ReciveFile(object networkStream)
        {
            NetworkStream _NetworkStream = (NetworkStream)networkStream;
            int bytes = 0;
            const int bufferSize = 8192;
            byte[] PathSize = new byte[4];
            int BytesToRead;

            bytes = _NetworkStream.Read(PathSize, 0, PathSize.Length);
            BytesToRead = BitConverter.ToInt32(PathSize, 0);
            byte[] bPath = new byte[BytesToRead];
            string Path;
            bytes = _NetworkStream.Read(bPath, 0, bPath.Length);
            Path = Encoding.UTF8.GetString(bPath);


            byte[] FileSize = new byte[4];
            bytes = _NetworkStream.Read(FileSize, 0, FileSize.Length);
            BytesToRead = BitConverter.ToInt32(FileSize, 0);
            bytes = 0;

            //////////////
            int count = BytesToRead / bufferSize;
            fileManagerForm.Set_progressBar_SendingProgress(0, count, 1);
            fileManagerForm.Set_HowMuchMustReceived(FileSizeAndMeasure((long)BytesToRead));
            fileManagerForm.Messege_Status("Reciving file...", 1);

            timer = new System.Threading.Timer(RecivingSpeedTimer, num, 0, 1000);
            ////////////

            while (true)
            {
                NetFile file;
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    do
                    {
                        int received = _NetworkStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, received);
                        bytes += received;
                        ///////////////////////
                        fileManagerForm.Change_progressBar_SendingProgress();
                        fileManagerForm.Set_HowMuchIsReceived(FileSizeAndMeasure((long)bytes));
                        Speed = Speed + received;
                        ///////////////////////
                    }
                    while (!(BytesToRead == bytes));
                    file = new NetFile(memStream.ToArray());
                }
                /////////
                timer.Change(Timeout.Infinite, 1000);
                fileManagerForm.Set_ReceivingSpeed("0 Byte/s");
                Speed = 0;
                fileManagerForm.Messege_Status("Creating file...", 1);
                ////////
                using (FileStream stream = new FileStream(Path + "\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(file.Data, 0, file.Data.Length);
                    MessageToUser.ShowInformation("File created!", "Client");
                }
                if(Path.Length == 2)
                {
                    Path = Path + "\\";
                }
                FileManagerInfoClient = new FileManagerInfo(Path);
                
                /////////
                ShowFM(FileManagerInfoClient, true);
                fileManagerForm.Reset_progressBar_SendingProgress();
                fileManagerForm.Set_HowMuchIsReceived(FileSizeAndMeasure(0));
                fileManagerForm.Set_HowMuchMustReceived(FileSizeAndMeasure(0));
                fileManagerForm.Messege_Status("Ready!", 1);
                ////////
            }
        }
        #endregion
    }
}
