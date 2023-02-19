using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using ClassLibraryRemotePC;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

using System.Runtime.Serialization.Formatters.Binary;


namespace Server
{
    class Presenter
    {
        MainForm mainForm;
        MessageToUser MessageToUser = new MessageToUser();
        TcpListener tcpListener;
        Thread ListenThread;
        OperationInterpreter operationInterpreter;
        OperationCod WaitForAdditionalConnectionOperationCod;
        string CurrentPath;

        // Сетевые переменные и потоки: 0 - Системные сообщения, 1 - Файловый менеджер, 2 - передача файлов, 3 - трансляция экрана, 4 - управление, 5 - текстовый чат
        List<List<TcpClient>> TcpClients = new List<List<TcpClient>>();
        List<List<NetworkStream>> NetworkStreams = new List<List<NetworkStream>>();
        List<List<Thread>> ReciveData = new List<List<Thread>>();

        int ServerPort; //49152
        string ServerPassword;
        string ServerIp;

        short NumPcToStartRemoteScreen;
        short NumPcToStopRemoteScreen;
        //bool IsFirstStartRemoteScreen = true;

        List<string> ClientIps = new List<string>();
        List<bool> IsClientesConnected = new List<bool>();

        List<short> ClientNumsWatchRemoteScreen = new List<short>();
        List<bool> IsFirstOpenRemoteScreen = new List<bool>();

        Bitmap ReciveImage;
        Thread CaptureScreenThread;
        static Semaphore CaptureScreenSem = new Semaphore(1, 1);
        Thread SendScreenImageThread;

        List<string> ReciveStrings = new List<string>();
        private static System.Object lockThis = new System.Object();

        public Presenter (MainForm _mainForm)
        {
            mainForm = _mainForm;

            List<string> Languages = new List<string>();
            Languages.Add("English");
            Languages.Add("Українська");
            Languages.Add("Русский");
            mainForm.SetComboBox_Language(Languages);
            mainForm.Set_Start_Stop_Listen_ButtonText("Start");


            operationInterpreter = new OperationInterpreter(true);

            operationInterpreter.Disconnecting += OperationInterpreter_Disconnecting;
            operationInterpreter.SendOperationToRemotePC += OperationInterpreter_SendOperationToRemotePC;
            operationInterpreter.WaitForAdditionalConnection += OperationInterpreter_WaitForAdditionalConnection;
            operationInterpreter.InformationAboutLogicalDrives_AndRootDirectoryObjects += OperationInterpreter_InformationAboutLogicalDrives_AndRootDirectoryObjects;
            operationInterpreter.InformationAboutLogicalDrives_AndСustomDirectoryObjects += OperationInterpreter_InformationAboutLogicalDrives_AndСustomDirectoryObjects;
            operationInterpreter.StartBroadcastingScreen += OperationInterpreter_StartBroadcastingScreen;
            operationInterpreter.StopBroadcastingScreen += OperationInterpreter_StopBroadcastingScreen;
            operationInterpreter.SendFile += OperationInterpreter_SendFile;
            operationInterpreter.DeleteFile += OperationInterpreter_DeleteFile;
            operationInterpreter.LaunchFile += OperationInterpreter_LaunchFile;

            mainForm.MainFormClosed += MainForm_MainFormClosed;
            mainForm.Start_Stop_Listen += MainForm_Start_Stop_Listen;
            mainForm.SendTextMessage += MainForm_SendTextMessage;
            mainForm.SendRemoteScreenImage += MainForm_SendRemoteScreenImage;

            #region Проверка начальных значений номера порта Ip адреса и пароля
            string PortStr = mainForm.GetPort();
            bool PortOk = false;
            int Port = 0;
            string Password = mainForm.GetPassword();
            bool PasswordOK = false;

            if (PortStr == "")
            {
                MessageToUser.ShowError("\"Port\" field empty!", "Server, Port Error");
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
                    MessageToUser.ShowError("\"Port\" field must contain only numbers!", "Server, Port Error");
                }
                if (PortOk == true)
                {
                    if (Port < 49152 || Port > 65535)
                    {
                        PortOk = false;
                        MessageToUser.ShowError("\"Port\" field must contain number in the range from 49152 to 65535!", "Server, Port Error");
                    }
                }
            }
            if (Password == "")
            {
                MessageToUser.ShowError("\"Password\" field empty!", "Server, Password Error");
            }
            else
            {
                if (Encoding.Unicode.GetBytes(Password).Length <= 100)
                {
                    PasswordOK = true;
                }
                else
                {
                    MessageToUser.ShowError("\"Password\" field must be between 1 and 50 characters!", "Server, Password Error");
                }
            }
            if (PortOk == true && PasswordOK == true)
            {
                ServerPassword = Password;
                ServerPort = Port;
                ListenThread = new Thread(ListenTcp);
                ListenThread.Start(ServerPort);
                mainForm.Set_Start_Stop_Listen_ButtonText("Stop");
            }
            #endregion

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            CurrentPath = directory.ToString();
            CurrentPath = CurrentPath + "\\";
        }

        public void WriteStringInFile(string StringToWrite)
        {
            lock (lockThis)
            {
                string Time = DateTime.Now.ToLongTimeString();
                using (StreamWriter streamWriter = File.AppendText(CurrentPath + "Server_log.txt"))
                {
                    streamWriter.WriteLine(Time + " ::" + StringToWrite);
                }
            }
        }
        private bool StopAllServerThreads()
        {
            bool res;
            try
            {
                if (ListenThread.IsAlive == true)
                {
                    tcpListener.Stop();
                    ListenThread.Abort();
                }
                foreach (List<Thread> threads in ReciveData)
                {
                    foreach (Thread thread in threads)
                    {
                        if (thread.IsAlive == true)
                        {
                            thread.Abort();
                        }
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
        private bool StopAllThreads()
        {
            bool res;
            bool IsSuccessfulStopModulThreads = operationInterpreter.StopAllThreadsInModul();
            bool IsSuccessfulStopSeverThreads = StopAllServerThreads();
            if (IsSuccessfulStopModulThreads == true && IsSuccessfulStopSeverThreads == true)
            {
                res = true;
            }
            else
            {
                res = false;
            }
            return res;
        }

        #region Обработка событий Интерпретатора КОП
        private void OperationInterpreter_InformationAboutLogicalDrives_AndСustomDirectoryObjects(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System","LD and custom dir info. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            FileManagerInfo FS_Info = new FileManagerInfo(operationCod.AdditionalParam_DirPath);
            SendFSInfo(FS_Info, operationCod.AdditionalParam_NumRemotePC);
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_InformationAboutLogicalDrives_AndRootDirectoryObjects(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "LD and root dir info. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            FileManagerInfo FS_Info = new FileManagerInfo();
            SendFSInfo(FS_Info, operationCod.AdditionalParam_NumRemotePC);
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_WaitForAdditionalConnection(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Wait For Additional Connection. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            WaitForAdditionalConnectionOperationCod = operationCod;
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_SendOperationToRemotePC(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Send Operation To RemotePC. Client " + operationCod.NumberPC, true);
            SendOperationCode(operationCod, operationCod.NumberPC);
        }
        private void OperationInterpreter_Disconnecting(object sender, EventArgs e)
        {
            //for (int i = 0; i < 6; i++)
            //{
            //    try
            //    {
            //        Disconnect(i);
            //    }
            //    catch
            //    {
            //        MessageToUser.ShowError("Disconnect " + i + " connection failed!", "Server");
            //    }
            //}
            OperationCod operationCod = (OperationCod)sender;
            mainForm.TextMessenger_AllMessenges("System", "Disconnect. Client " + operationCod.AdditionalParam_NumRemotePC, true);
        }//Переделать!!!
        private void OperationInterpreter_StopBroadcastingScreen(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Stop Broadcasting Screen. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            NumPcToStopRemoteScreen = operationCod.AdditionalParam_NumRemotePC;

            WriteStringInFile("Stop broadcasting screen to " + ClientIps[NumPcToStopRemoteScreen] + " client");
            mainForm.Messege_Status("Stop broadcasting screen to " + ClientIps[NumPcToStopRemoteScreen] + " client", 3);

            if (ClientNumsWatchRemoteScreen.Count == 1)
            {
                CaptureScreenThread.Abort();
                mainForm.Stop_RemoteScreenTimer();
                ClientNumsWatchRemoteScreen.Remove(NumPcToStopRemoteScreen);
            }
            if (ClientNumsWatchRemoteScreen.Count > 1)
            {
                CaptureScreenSem.WaitOne();
                CaptureScreenThread.Suspend();
                ClientNumsWatchRemoteScreen.Remove(NumPcToStopRemoteScreen);
                CaptureScreenThread.Resume();
                CaptureScreenSem.Release();
            }
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_StartBroadcastingScreen(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Start Broadcasting Screen. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            NumPcToStartRemoteScreen = operationCod.AdditionalParam_NumRemotePC;

            if (ClientNumsWatchRemoteScreen.Count > 0)
            {
                CaptureScreenSem.WaitOne();
                CaptureScreenThread.Suspend();
                ClientNumsWatchRemoteScreen.Add(NumPcToStartRemoteScreen);
                CaptureScreenThread.Resume();
                CaptureScreenSem.Release();
            }
            else if(ClientNumsWatchRemoteScreen.Count == 0)
            {
                ClientNumsWatchRemoteScreen.Add(NumPcToStartRemoteScreen);
            }

            WriteStringInFile("Start broadcasting screen to " + ClientIps[NumPcToStartRemoteScreen] + " client");
            mainForm.Messege_Status("Start broadcasting screen to " + ClientIps[NumPcToStartRemoteScreen] + " client", 1);

            if (ClientNumsWatchRemoteScreen.Count == 1)
            {
                CaptureScreenThread = new Thread(mainForm.CaptureScreen);
                CaptureScreenThread.Start();
                mainForm.Set_RemoteScreenTimer_interval(13);
                mainForm.Start_RemoteScreenTimer();
            }
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_SendFile(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Send File. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            SendFile(operationCod.AdditionalParam_NumRemotePC, operationCod.AdditionalParam_DirPath, operationCod.AdditionalParam_DirPath2);
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_DeleteFile(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
            //mainForm.TextMessenger_AllMessenges("System", "Send File. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            int res = DeleteFile(operationCod.AdditionalParam_DirPath);
            if(res == 1)
            {
                WriteStringInFile("Deleting file (" + operationCod.AdditionalParam_DirPath + ") Client: " + ClientIps[operationCod.AdditionalParam_NumRemotePC]);
                mainForm.Messege_Status("Deleting file (" + operationCod.AdditionalParam_DirPath + ") Client: " + ClientIps[operationCod.AdditionalParam_NumRemotePC], 3);
            }
            else if(res == 0)
            {
                WriteStringInFile("File (" + operationCod.AdditionalParam_DirPath + ") doesn't exist. Client: " + ClientIps[operationCod.AdditionalParam_NumRemotePC]);
                mainForm.Messege_Status("File (" + operationCod.AdditionalParam_DirPath + ") doesn't exist. Client: " + ClientIps[operationCod.AdditionalParam_NumRemotePC], 2);
            }
            else if(res == -1)
            {
                WriteStringInFile("Delete file (" + operationCod.AdditionalParam_DirPath + ") failed. Client: " + ClientIps[operationCod.AdditionalParam_NumRemotePC]);
                mainForm.Messege_Status("Delete file (" + operationCod.AdditionalParam_DirPath + ") failed. Client: " + ClientIps[operationCod.AdditionalParam_NumRemotePC], 2);
            }
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        private void OperationInterpreter_LaunchFile(object sender, EventArgs e)
        {
            OperationCod operationCod = (OperationCod)sender;
           // mainForm.TextMessenger_AllMessenges("System", "Launch File. Client " + operationCod.AdditionalParam_NumRemotePC, true);
            int client_num = operationCod.AdditionalParam_NumRemotePC;
            string path = operationCod.AdditionalParam_DirPath;
            Process p = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.UseShellExecute = true;
            pi.FileName = path;
            p.StartInfo = pi;

            try
            {
                p.Start();
                WriteStringInFile("Launch file (" + path + ") Client: " + ClientIps[client_num]);
                mainForm.Messege_Status("Launch file (" + path + ") Client: " + ClientIps[client_num], 3);
            }
            catch (Exception Ex)
            {
                WriteStringInFile("Launch file (" + path + ") failed. Client: " + ClientIps[client_num]);
                mainForm.Messege_Status("Launch file (" + path + ") failed. Client: " + ClientIps[client_num], 2);
            }
            operationInterpreter.AddForAnalysisOperationCode(operationCod);
        }
        #endregion

        #region Обработка событий Формы
        private void MainForm_Start_Stop_Listen(object sender, EventArgs e)
        {
            if (mainForm.Get_Start_Stop_Listen_ButtonText() == "Start")
            {
                string PortStr = mainForm.GetPort();
                bool PortOk = false;
                int Port = 0;
                string Password = mainForm.GetPassword();
                bool PasswordOK = false;

                if (PortStr == "")
                {
                    MessageToUser.ShowError("\"Port\" field empty!", "Server, Port Error");
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
                        MessageToUser.ShowError("\"Port\" field must contain only numbers!", "Server, Port Error");
                    }
                    if (PortOk == true)
                    {
                        if (Port < 49152 || Port > 65535)
                        {
                            PortOk = false;
                            MessageToUser.ShowError("\"Port\" field must contain number in the range from 49152 to 65535!", "Server, Port Error");
                        }
                    }
                }
                if (Password == "")
                {
                    MessageToUser.ShowError("\"Password\" field empty!", "Server, Password Error");
                }
                else
                {
                    if (Encoding.Unicode.GetBytes(Password).Length <= 100)
                    {
                        PasswordOK = true;
                    }
                    else
                    {
                        MessageToUser.ShowError("\"Password\" field must be between 1 and 50 characters!", "Server, Password Error");
                    }
                }
                if (PortOk == true && PasswordOK == true)
                {
                    ServerPassword = Password;
                    ServerPort = Port;
                    ListenThread = new Thread(ListenTcp);
                    ListenThread.Start(ServerPort);
                    mainForm.Set_Start_Stop_Listen_ButtonText("Stop");
                }
            }
            else if(mainForm.Get_Start_Stop_Listen_ButtonText() == "Stop")
            {
                StopAllServerThreads();
                
                for (int i = 0; i < TcpClients.Count(); i++)
                {
                    Disconnect(i);
                }
                mainForm.Set_Start_Stop_Listen_ButtonText("Start");
                WriteStringInFile("Server Stopped!");
                mainForm.Messege_Status("Server Stopped!", 1);
            }
        }
        private void MainForm_MainFormClosed(object sender, EventArgs e)
        {
            StopAllThreads();
        }
        private void MainForm_SendTextMessage(object sender, EventArgs e)
        {
            string messege = mainForm.Get_TextMessenge();
            if (messege != "")
            {
                for (short j = 0; j < NetworkStreams.Count(); j++)
                { 
                    SendTextMessage(messege, j);
                }
                string NameDate = DateTime.Now.ToLongTimeString() + " - " + "Server (" + ServerIp + "): ";
                mainForm.TextMessenger_AllMessenges(NameDate, messege, true);
            }
        }
        private void MainForm_SendRemoteScreenImage(object sender, EventArgs e)
        {
            ReciveImage = (Bitmap)sender;
           // Image im = GetCompressedBitmap(ReciveImage, 10L);
            //ReciveImage = new Bitmap(im);
            NetImage netImage = new NetImage(ReciveImage);

            CaptureScreenSem.WaitOne();
            foreach (short num in ClientNumsWatchRemoteScreen)
            {
                SendScreenImage(netImage, num);
            }
            CaptureScreenSem.Release();
        }
        #endregion

        #region Подключение и отключение
        public void ListenTcp(object _port)
        {
            int port = Convert.ToInt32(_port);
            TcpClient _tcpClient;
            bool flg = false;
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            WriteStringInFile("Server started! Port number " + port + ". Waiting connection...");
            mainForm.Messege_Status("Server started! Port number " + port + ". Waiting connection...", 1);

            while(true)
            {
                bool IsClientConnected = false;
                short NumFindClient = -1; // -1 New client

                _tcpClient = tcpListener.AcceptTcpClient();
                if (flg == false)
                {
                    ServerIp = IPAddress.Parse(((IPEndPoint)_tcpClient.Client.LocalEndPoint).Address.ToString()).ToString();
                    flg = true;
                }
                for (short i = 0; i < ClientIps.Count; i++)
                {
                    if (ClientIps[i] == IPAddress.Parse(((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString()).ToString())
                    {
                        IsClientConnected = IsClientesConnected[i];
                        NumFindClient = i;
                        break;
                    }
                }//поиск даного клиента в списке клиентов

                if (IsClientConnected == false)
                {
                    StringBuilder ClientPassword = new StringBuilder();
                    byte[] data = new byte[100];
                    int bytes = 0;
                    bytes = _tcpClient.GetStream().Read(data, 0, data.Length);
                    ClientPassword.Append(Encoding.Unicode.GetString(data, 0, bytes));


                    if (ServerPassword == ClientPassword.ToString())
                    {
                        TcpClients.Add(new List<TcpClient>());
                        NetworkStreams.Add(new List<NetworkStream>());
                        ReciveData.Add(new List<Thread>());

                        for (int i = 0; i < 6; i++)
                        {
                            TcpClients[TcpClients.Count() - 1].Add(null);
                            NetworkStreams[NetworkStreams.Count() - 1].Add(null);
                            ReciveData[ReciveData.Count() - 1].Add(null);
                        }

                        ClientIps.Add(IPAddress.Parse(((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString()).ToString());

                        TcpClients[TcpClients.Count() - 1][0] = _tcpClient;
                        NetworkStreams[NetworkStreams.Count() - 1][0] = _tcpClient.GetStream();

                        IsClientesConnected.Add(true);

                        data = Encoding.Unicode.GetBytes("Good!");
                        _tcpClient.GetStream().Write(data, 0, data.Length);

                        byte[] NumClientOnServer = BitConverter.GetBytes((short)(ClientIps.Count() - 1));
                        _tcpClient.GetStream().Write(NumClientOnServer, 0, 2);

                        data = Encoding.Unicode.GetBytes(ClientIps[ClientIps.Count() - 1]);
                        _tcpClient.GetStream().Write(data, 0, data.Length);

                        ReciveData[ReciveData.Count() - 1][0] = new Thread(ReciveOperationCode);
                        ReciveData[ReciveData.Count() - 1][0].Start((short)(ClientIps.Count() - 1));
                        WriteStringInFile("Client " + ClientIps[ClientIps.Count() - 1] + " connected!");
                        mainForm.Messege_Status("Client " + ClientIps[ClientIps.Count() - 1] + " connected!", 1);

                        //автоматическое создание подключения для текстового чата
                        OperationCod Cod_TextChat_Request = operationInterpreter.CodeWork_TextChat_Request(-1);
                        Cod_TextChat_Request.AdditionalParam_NumRemotePC = (short)(ClientIps.Count() - 1);
                        operationInterpreter.AddForAnalysisOperationCode(Cod_TextChat_Request);
                    }
                    else
                    {
                        WriteStringInFile("Client " + IPAddress.Parse(((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString()).ToString() + " trying to connect with wrong \"" + ClientPassword.ToString() + "\" password!");
                        mainForm.Messege_Status("Client " + IPAddress.Parse(((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString()).ToString() + " trying to connect with wrong \"" + ClientPassword.ToString() + "\" password!", 3);
                        data = Encoding.Unicode.GetBytes("Bad!");
                        _tcpClient.GetStream().Write(data, 0, data.Length);
                    }
                }//первое подключение (системные сообщения)
                else
                {
                    //if (ClientIps[NumFindClient] == IPAddress.Parse(((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString()).ToString())
                    //{
                        if (WaitForAdditionalConnectionOperationCod.OperationTypes[1] == 1 && WaitForAdditionalConnectionOperationCod.ChainPartNumber == 0)
                        {
                            TcpClients[NumFindClient][1] = _tcpClient;
                            NetworkStreams[NumFindClient][1] = _tcpClient.GetStream();
                        }//первое доп. подключение файлового менеджера. Инфа о ФС.
                        else if (WaitForAdditionalConnectionOperationCod.OperationTypes[1] == 1 && WaitForAdditionalConnectionOperationCod.ChainPartNumber == 1)
                        {
                            TcpClients[NumFindClient][2] = _tcpClient;
                            NetworkStreams[NumFindClient][2] = _tcpClient.GetStream();
                            ReciveData[NumFindClient][2] = new Thread(ReciveFile);
                            ReciveData[NumFindClient][2].Start(NumFindClient);
                        }//второе доп. подключение файлового менеджера. Передача файлов.
                        else if (WaitForAdditionalConnectionOperationCod.OperationTypes[1] == 2 && WaitForAdditionalConnectionOperationCod.ChainPartNumber == 0)
                        {
                            TcpClients[NumFindClient][3] = _tcpClient;
                            NetworkStreams[NumFindClient][3] = _tcpClient.GetStream();
                        }//доп. подключение трансляции экрана.
                        else if (WaitForAdditionalConnectionOperationCod.OperationTypes[1] == 5 && WaitForAdditionalConnectionOperationCod.ChainPartNumber == 0)
                        {
                            TcpClients[NumFindClient][5] = _tcpClient;
                            NetworkStreams[NumFindClient][5] = _tcpClient.GetStream();
                            ReciveData[NumFindClient][5] = new Thread(ReceiveTextMessage);
                            ReciveData[NumFindClient][5].Start(NumFindClient);
                        }//Текстовый чат


                   // }//если этот тот самый клиент
                   
                }//дополнительное подключение
            }
        }
        void Disconnect(int num)
        {
            for (int i = 0; i < 6; i++)
            {
                if (NetworkStreams[num][i] != null)
                    NetworkStreams[num][i].Close();
                if (TcpClients[num][i] != null)
                    TcpClients[num][i].Close();
                if (ReciveData[num][i] != null)
                    ReciveData[num][i].Abort();
                //Environment.Exit(0);
            }
        }
        #endregion

        #region Системные сообщения
        private void ReciveOperationCode(object pcNum)
        {
            short num = (short)pcNum;
            NetworkStream _networkStream = NetworkStreams[num][0];
                int bytes;
                const int bufferSize = 8192;
                int BytesToRead;

                while (true)
                {
                    byte[] OpCodeSize = new byte[4];
                    bytes = _networkStream.Read(OpCodeSize, 0, OpCodeSize.Length);
                    BytesToRead = BitConverter.ToInt32(OpCodeSize, 0);
                    bytes = 0;

                    OperationCod OpCod;
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        byte[] buffer = new byte[bufferSize];
                        do
                        {
                            int received = _networkStream.Read(buffer, 0, buffer.Length);
                            memStream.Write(buffer, 0, received);
                            bytes += received;
                        }
                        while (!(BytesToRead == bytes));
                        OpCod = new OperationCod(memStream.ToArray());
                        if (OpCod.OperationStatus != 1)
                        {
                            OpCod.AdditionalParam_NumRemotePC = OpCod.NumberPC;
                        }
                        OpCod.NumberLinkedPC = -1;
                        operationInterpreter.AddForAnalysisOperationCode(OpCod);
                    }
                }
        }
        private void SendOperationCode(OperationCod operationCod, short pcNum)
        {
            NetworkStream _networkStream = NetworkStreams[pcNum][0];

            byte[] SendOpCode = operationCod.ToArray();
            byte[] OpCodeSize = BitConverter.GetBytes(SendOpCode.Length);
            _networkStream.Write(OpCodeSize, 0, 4);
            _networkStream.Write(SendOpCode, 0, SendOpCode.Length);
        }
        #endregion

        #region Текстовые сообщения
        protected internal void ReceiveTextMessage(object pcNum)
        {
            short num = (short)pcNum;
            NetworkStream _networkStream = NetworkStreams[num][5];

            while (true)
            {
                try
                {
                    byte[] data = new byte[512];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = _networkStream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_networkStream.DataAvailable);

                    string NameDate = DateTime.Now.ToLongTimeString() + " - " + "Client ("+ ClientIps[num] + "): ";
                    mainForm.TextMessenger_AllMessenges(NameDate, builder.ToString(), false);
                }
                catch
                {
                    Disconnect(5);
                }
            }
        }
        void SendTextMessage(string message, short pcNum)
        {
            NetworkStream _networkStream = NetworkStreams[pcNum][5];

            byte[] data1 = Encoding.Unicode.GetBytes(message);
            _networkStream.Write(data1, 0, data1.Length);
        }
        #endregion

        #region Трансляция экрана
        private void ReciveScreenImage(object pcNum)
        {
            short num = (short)pcNum;
            NetworkStream _networkStream = NetworkStreams[num][3];

            int bytes;
            const int bufferSize = 8192;
            int BytesToRead;

            while (true)
            {
                byte[] OpCodeSize = new byte[4];
                bytes = _networkStream.Read(OpCodeSize, 0, OpCodeSize.Length);
                BytesToRead = BitConverter.ToInt32(OpCodeSize, 0);
                bytes = 0;

                NetImage Image;
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    do
                    {
                        int received = _networkStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, received);
                        bytes += received;
                    }
                    while (!(BytesToRead == bytes));
                    Image = new NetImage(memStream.ToArray());
                    ReciveImage = Image.image;
                }
            }
        }
        private void SendScreenImage(NetImage _image, short pcNum)
        {
            NetworkStream _networkStream = NetworkStreams[pcNum][3];

            byte[] SendImage = _image.ToArray();
            byte[] ImageSize = BitConverter.GetBytes(SendImage.Length);
            _networkStream.Write(ImageSize, 0, 4);
            _networkStream.Write(SendImage, 0, SendImage.Length);
        }
        private Image GetCompressedBitmap(Bitmap bmp, long quality)
        {
            using (var mss = new MemoryStream())
            {
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = qualityParam;
                bmp.Save(mss, imageCodec, parameters);
                return Image.FromStream(mss);
            }
        }
        #endregion

        #region Файловая система
        protected internal void ReceiveFSInfo(object pcNum)
        {
            short num = (short)pcNum;
            NetworkStream _networkStream = NetworkStreams[num][1];

            int bytes;
            const int bufferSize = 8192;
            int BytesToRead;

            while (true)
            {
                byte[] FS_Info_Size = new byte[4];
                bytes = _networkStream.Read(FS_Info_Size, 0, FS_Info_Size.Length);
                BytesToRead = BitConverter.ToInt32(FS_Info_Size, 0);
                bytes = 0;

                FileManagerInfo FS_Info;
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    do
                    {
                        int received = _networkStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, received);
                        bytes += received;
                    }
                    while (!(BytesToRead == bytes));
                    FS_Info = new FileManagerInfo(memStream.ToArray());
                }
            }
        }
        private void SendFSInfo(FileManagerInfo FS_Info, short pcNum)
        {
            NetworkStream _networkStream = NetworkStreams[pcNum][1];

            FS_Info.LastWriteTime = new List<DateTime>();
            FS_Info.Attributes = new List<FileAttributes>();
            FS_Info.LengthObj = new List<long>();
            foreach (FileSystemInfo fs in FS_Info.fileSystemObjectsInfo)
            {
                FS_Info.LastWriteTime.Add(fs.LastWriteTime);
                FS_Info.Attributes.Add(fs.Attributes);
                try
                {
                    FileInfo fileInfo = new FileInfo(fs.FullName);
                    FS_Info.LengthObj.Add(fileInfo.Length);
                }
                catch
                {
                    FS_Info.LengthObj.Add(-1);
                }
            }

            byte[] Send_FS_Info = FS_Info.ToArray();
            byte[] FS_Info_Size = BitConverter.GetBytes(Send_FS_Info.Length);
            _networkStream.Write(FS_Info_Size, 0, 4);
            _networkStream.Write(Send_FS_Info, 0, Send_FS_Info.Length);
        }
        /// <summary>
        /// Удаляет файл по указанному пути.
        /// </summary>
        /// <param name="path">Путь к файлу, коорый нужно удалить.</param>
        /// <returns>Номер статуса операции. 1 - файл удален, 0 - файл не существует, -1 - не удалось удалить файл.</returns>
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
        void ReciveFile(object _pcNum)
        {
            short pcNum = (short)_pcNum;
            NetworkStream _NetworkStream = NetworkStreams[pcNum][2];
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

            WriteStringInFile("Reciving file which will be written in dir :" + Path + ", size: "+ BytesToRead + " from " + ClientIps[pcNum] + " client");
            mainForm.Messege_Status("Reciving file which will be written in dir :" + Path + ", size: "+ BytesToRead + " from " + ClientIps[pcNum] + " client", 1);

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
                    }
                    while (!(BytesToRead == bytes));
                    file = new NetFile(memStream.ToArray());
                }

                using (FileStream stream = new FileStream(Path + "\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(file.Data, 0, file.Data.Length);
                    WriteStringInFile("Creating file (" + Path + "\\" + file.FileName + ") from " + ClientIps[pcNum] + " client");
                    mainForm.Messege_Status("Creating file (" + Path + "\\" + file.FileName + ") from " + ClientIps[pcNum] + " client", 1);
                }
                OperationCod CodFileManagerUserDir = operationInterpreter.Code_InformationAboutLogicalDrives_AndСustomDirectoryObjects(pcNum, Path);
                CodFileManagerUserDir.AdditionalParam_NumRemotePC = CodFileManagerUserDir.NumberPC;
                CodFileManagerUserDir.NumberLinkedPC = -1;
                operationInterpreter.AddForAnalysisOperationCode(CodFileManagerUserDir);
            }
        }
        public void SendFile(short pcNum, string _SendFilePath, string _WriteFilePath)
        {
            NetworkStream _networkStream = NetworkStreams[pcNum][2];

            WriteStringInFile("Send file (" + _SendFilePath + ") to " + ClientIps[pcNum] + " client");
            mainForm.Messege_Status("Send file (" + _SendFilePath + ") to " + ClientIps[pcNum] + " client", 1);
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
        #endregion

    }
}
