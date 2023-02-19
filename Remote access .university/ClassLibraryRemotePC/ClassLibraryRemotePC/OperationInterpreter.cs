using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ClassLibraryRemotePC
{
    public class OperationInterpreter
    {
        List<OperationCod> OnExecutionOperationCodes = new List<OperationCod>();
        List<OperationCod> ForAnalysisOperationCodes = new List<OperationCod>();

        List<OperationCod> OnExecutionOperationCodes_AddingBuff = new List<OperationCod>();
        List<OperationCod> ForAnalysisOperationCodes_AddingBuff = new List<OperationCod>();

        bool IsServer;
        Thread InputInterpreterOperation;
        Thread OutputInterpreterOperation;
        OperationControl operationControl;
        Thread СhoiceOperationToRun;
        public event EventHandler SendOperationToRemotePC;

        internal int OnExecutionThreadSleepingTime
        { set; get; } = 75;

        internal int ForAnalysisThreadSleepingTime
        { set; get; } = 75;

        static Semaphore ForAnalysisSem = new Semaphore(1, 1);
        static Semaphore OnExecutionSem = new Semaphore(1, 1);

        #region События Сеть (OperationTypes[0] == 0) 
        public event EventHandler WaitForAdditionalConnection; //0
        public event EventHandler InitiateAdditionalConnection; //1
        public event EventHandler Disconnecting; //2
        public event EventHandler SuccessfulRegistrationOnServer; //3
        #endregion

        #region События Файловый Менеджер (OperationTypes[0] == 1) 
        public event EventHandler InformationAboutLogicalDrives_AndRootDirectoryObjects; //0
        public event EventHandler InformationAboutLogicalDrives_AndСustomDirectoryObjects; //1
        public event EventHandler DeleteFile; //3
        public event EventHandler LaunchFile; //4
        #endregion

        #region События Трансляция экрана (OperationTypes[0] == 2) 
        public event EventHandler StartBroadcastingScreen; //0
        public event EventHandler StopBroadcastingScreen; //1
        #endregion

        #region События Управление удаленным ПК (OperationTypes[0] == 3) 

        #endregion

        #region События Передача файлов (OperationTypes[0] == 4) 
        public event EventHandler SendFile; //0
        #endregion

        public OperationInterpreter(bool _IsServer)
        {
            IsServer = _IsServer;

            if (IsServer == true)
            {
                operationControl = new OperationControl();
                operationControl.RunOperationCode += OperationControl_RunOperationCode;
                СhoiceOperationToRun = new Thread(operationControl.OperationControlModul);
                СhoiceOperationToRun.Start();
                InputInterpreterOperation = new Thread(this.AnalysisOperationCodes);
                InputInterpreterOperation.Start();
                OutputInterpreterOperation = new Thread(this.ExecutionOperationInterpretator_Server);
                OutputInterpreterOperation.Start();
            }
            else
            {
                OutputInterpreterOperation = new Thread(this.ExecutionOperationInterpretator_Client);
                OutputInterpreterOperation.Start();
            }
        }
        public bool StopAllThreadsInModul()
        {
            bool res;
            try
            {
                if (InputInterpreterOperation.IsAlive == true)
                {
                    InputInterpreterOperation.Abort();
                }
                if (OutputInterpreterOperation.IsAlive == true)
                {
                    OutputInterpreterOperation.Abort();
                }
                if (СhoiceOperationToRun.IsAlive == true)
                {
                    СhoiceOperationToRun.Abort();
                }
                res = true;
            }
            catch
            {
                res = false;
            }
            return res;
        }

        #region Выходные КОП
        public void AddOnExecutionOperationCode(OperationCod _operationCod)
        {
            OnExecutionOperationCodes_AddingBuff.Add(_operationCod);
        }
        private void OperationControl_RunOperationCode(object sender, EventArgs e)
        {
            AddOnExecutionOperationCode((OperationCod)sender);
        }
        private void InterpreteExecutionOperation(OperationCod operationCod)
        {
            if(operationCod.OperationTypes[0] == 0)
            {
                if (operationCod.OperationNumber == 0)
                {
                    if (WaitForAdditionalConnection != null) WaitForAdditionalConnection(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 1)
                {
                    if (InitiateAdditionalConnection != null) InitiateAdditionalConnection(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 2)
                {
                    if (Disconnecting != null) Disconnecting(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 3)
                {
                    if (SuccessfulRegistrationOnServer != null) SuccessfulRegistrationOnServer(operationCod, EventArgs.Empty);
                }
            }//Сеть
            else if(operationCod.OperationTypes[0] == 1)
            {
                if (operationCod.OperationNumber == 0)
                {
                    if (InformationAboutLogicalDrives_AndRootDirectoryObjects != null) InformationAboutLogicalDrives_AndRootDirectoryObjects(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 1)
                {
                    if (InformationAboutLogicalDrives_AndСustomDirectoryObjects != null) InformationAboutLogicalDrives_AndСustomDirectoryObjects(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 3)
                {
                    if (DeleteFile != null) DeleteFile(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 4)
                {
                    if (LaunchFile != null) LaunchFile(operationCod, EventArgs.Empty);
                }
            }//Файловый менеджер
            else if(operationCod.OperationTypes[0] == 2)
            {
                if (operationCod.OperationNumber == 1)
                {
                    if (StartBroadcastingScreen != null) StartBroadcastingScreen(operationCod, EventArgs.Empty);
                }
                else if (operationCod.OperationNumber == 2)
                {
                    if (StopBroadcastingScreen != null) StopBroadcastingScreen(operationCod, EventArgs.Empty);
                }
            }//Трансляция экрана
            else if (operationCod.OperationTypes[0] == 3)
            {

            }//Управление. Клавиатура и мышь.
            else if (operationCod.OperationTypes[0] == 4)
            {
                if (SendFile != null) SendFile(operationCod, EventArgs.Empty);
            }//передача файлов
            else if (operationCod.OperationTypes[0] == 5)
            {

            }//текстовый чат
        }//интерпретация коп на события (клиент и сервер).
        private void InterpreteExecutionOperationCodeServer(OperationCod operationCod)
        {
            if (operationCod.NumberPC == -1)
            {
                InterpreteExecutionOperation(operationCod);
            }
            else
            {
                if (SendOperationToRemotePC != null) SendOperationToRemotePC(operationCod, EventArgs.Empty);
            }
        }
        private void ExecutionOperationInterpretator_Server()
        {
            while (true)
            {
                OnExecutionSem.WaitOne();
                foreach (OperationCod operationCod in OnExecutionOperationCodes)
                {
                    InterpreteExecutionOperationCodeServer(operationCod);
                }
                OnExecutionOperationCodes.Clear();
                if (OnExecutionOperationCodes_AddingBuff.Count() > 0)
                {
                    OnExecutionOperationCodes.AddRange(OnExecutionOperationCodes_AddingBuff);
                    OnExecutionOperationCodes_AddingBuff.Clear();
                }
                OnExecutionSem.Release();
                Thread.Sleep(OnExecutionThreadSleepingTime);
            }
        }
        private void ExecutionOperationInterpretator_Client()
        {
            while (true)
            {
                OnExecutionSem.WaitOne();
                foreach (OperationCod operationCod in OnExecutionOperationCodes)
                {
                    InterpreteExecutionOperation(operationCod);
                }
                OnExecutionOperationCodes.Clear();
                if (OnExecutionOperationCodes_AddingBuff.Count() > 0)
                {
                    OnExecutionOperationCodes.AddRange(OnExecutionOperationCodes_AddingBuff);
                    OnExecutionOperationCodes_AddingBuff.Clear();
                }
                OnExecutionSem.Release();
                Thread.Sleep(OnExecutionThreadSleepingTime);
            }
        }
        #endregion

        #region Входные КОП.
        public void AddForAnalysisOperationCode(OperationCod _operationCod)
        {
            ForAnalysisOperationCodes_AddingBuff.Add(_operationCod);
        }
        private void AnalysisOperationCode(OperationCod operationCod)
        {
            if (operationCod.OperationStatus == 0)
            {
                if(operationCod.OperationTypes[0] == 0)
                {
                    if(operationCod.OperationNumber == 4)
                    {
                        List<OperationCod> DisconnectingChain = CodeDisconnectClientServer(operationCod.AdditionalParam_NumRemotePC);
                        foreach(OperationCod cod in DisconnectingChain)
                        {
                            operationControl.AddOperationCod(cod);
                        }
                    }//Запрос на отключение
                    else
                    {
                        operationControl.AddOperationCod(operationCod);
                    }//остальные коп типа "сеть"
                }//Сеть.
                else if(operationCod.OperationTypes[0] == 1)
                {
                    if(operationCod.OperationNumber == 2)
                    {
                        List<OperationCod> AdditionalConnection = CodeWaitAndInitiateAdditionalConnection(operationCod.AdditionalParam_NumRemotePC, 0, 1, 2);
                        foreach (OperationCod cod in AdditionalConnection)
                        {
                            operationControl.AddOperationCod(cod);
                        }
                        AdditionalConnection = CodeWaitAndInitiateAdditionalConnection(operationCod.AdditionalParam_NumRemotePC, 1, 1, 2);
                        foreach (OperationCod cod in AdditionalConnection)
                        {
                            operationControl.AddOperationCod(cod);
                        }
                        OperationCod FsCode = Code_Chain_InformationAboutLogicalDrives_AndRootDirectoryObjects(operationCod.NumberLinkedPC,
                            operationCod.NumberPC, 2, 2);
                        operationControl.AddOperationCod(FsCode);
                    }//запрос о работе с фм
                    else if((operationCod.OperationNumber == 1 || operationCod.OperationNumber == 0 ||
                        operationCod.OperationNumber == 3 || operationCod.OperationNumber == 4) && operationCod.IsChain == false)
                    {
                        operationCod.NumberPC = -1;
                        operationControl.AddOperationCod(operationCod);
                    }
                    else
                    {
                        operationControl.AddOperationCod(operationCod);
                    }//остальные коп типа "Файловый менеджер"
                }//Файловый менеджер.
                else if(operationCod.OperationTypes[0] == 2)
                {
                    if (operationCod.OperationNumber == 0)
                    {
                        List<OperationCod> AdditionalConnection = CodeWaitAndInitiateAdditionalConnection(operationCod.AdditionalParam_NumRemotePC, 0, 2, 1);
                        foreach (OperationCod cod in AdditionalConnection)
                        {
                            operationControl.AddOperationCod(cod);
                        }
                        OperationCod CodeStartRemoteScreen = Code_Chain_StartRemoteScreen(-1, operationCod.NumberPC, 1, 1);
                        operationControl.AddOperationCod(CodeStartRemoteScreen);
                    }//запрос на работу с Трансляцией экрана
                    else if(operationCod.OperationNumber == 2)
                    {
                        operationCod.NumberPC = -1;
                        operationControl.AddOperationCod(operationCod);
                    }
                    else if (operationCod.OperationNumber == 1)
                    {
                        operationCod.NumberPC = -1;
                        operationControl.AddOperationCod(operationCod);
                    }
                    else
                    {
                        operationControl.AddOperationCod(operationCod);
                    }//остальные коп типа "Трансляция экрана"
                }//Трансляция экрана
                else if (operationCod.OperationTypes[0] == 3)
                {

                }//Управление. Клавиатура и мышь.
                else if (operationCod.OperationTypes[0] == 4)
                {
                    if(operationCod.OperationNumber == 0)
                    {
                        operationCod.NumberPC = -1;
                        operationControl.AddOperationCod(operationCod);
                    }//передать файл
                    else
                    {
                        operationControl.AddOperationCod(operationCod);
                    }
                }//передача файлов
                else if (operationCod.OperationTypes[0] == 5)
                {
                    if (operationCod.OperationNumber == 0)
                    {
                        List<OperationCod> AdditionalConnection = CodeWaitAndInitiateAdditionalConnection(operationCod.AdditionalParam_NumRemotePC, 0, 5, 0);
                        foreach (OperationCod cod in AdditionalConnection)
                        {
                            operationControl.AddOperationCod(cod);
                        }
                    }//запрос на работу с текстовым чатом
                    else
                    {
                        operationControl.AddOperationCod(operationCod);
                    }//остальные коп типа "текстовый чат"
                }//текстовый чат

            }//Входной КОП с операцией или запросом. 
            else if (operationCod.OperationStatus == 1 && IsServer == true)
            {
                СhoiceOperationToRun.Suspend();
                operationControl.СonfirmOperationCod(operationCod);
                СhoiceOperationToRun.Resume();
            }//Подтверждение выполнения КОП.
        }//входные КОП (операции или запросы). Выполняется у сервера.
        private void AnalysisOperationCodes()
        {
            while (true)
            {
                ForAnalysisSem.WaitOne();
                foreach (OperationCod cod in ForAnalysisOperationCodes)
                {
                    AnalysisOperationCode(cod);
                }
                ForAnalysisOperationCodes.Clear();
                if (ForAnalysisOperationCodes_AddingBuff.Count() > 0)
                {
                    ForAnalysisOperationCodes.AddRange(ForAnalysisOperationCodes_AddingBuff);
                    ForAnalysisOperationCodes_AddingBuff.Clear();
                }
                ForAnalysisSem.Release();
                Thread.Sleep(ForAnalysisThreadSleepingTime);
            }
        }
        #endregion

        #region Список КОП

        #region Сеть (0)
        internal List<OperationCod> CodeWaitAndInitiateAdditionalConnection(short ClientNumber, short ChainPartNumber, ushort AdditionalOperationType, short TotalPartsInChain)
        {
            List<OperationCod> Codes = new List<OperationCod>();
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(0);
            operationTypes.Add(AdditionalOperationType);
            short OpStatus = 0;
            if(ChainPartNumber > 0)
            {
                OpStatus = -1;
            }
            OperationCod waitForAdditionalConnection = new OperationCod(-1, operationTypes, 0, OpStatus, true, ClientNumber, ChainPartNumber, TotalPartsInChain, 0, 1);
            waitForAdditionalConnection.AdditionalParam_NumRemotePC = ClientNumber;
            Codes.Add(waitForAdditionalConnection);
            OperationCod initiateAdditionalConnection = new OperationCod(ClientNumber, operationTypes, 1, -1, true, -1, ChainPartNumber, TotalPartsInChain, 1, 1);
            Codes.Add(initiateAdditionalConnection);
            return Codes;
        }
        internal List<OperationCod> CodeDisconnectClientServer(short ClientNumber)
        {
            List<OperationCod> Codes = new List<OperationCod>();
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(0);
            OperationCod InitiationDisconnectingClient = new OperationCod(ClientNumber, operationTypes, 2, 0, true, -1, 0, 0, 0, 1);
            Codes.Add(InitiationDisconnectingClient);
            OperationCod DisconnectClientOnServer = new OperationCod(-1, operationTypes, 2, -1, true, ClientNumber, 0, 0, 1, 1);
            Codes.Add(DisconnectClientOnServer);
            return Codes;
        }
        public OperationCod CodeSuccessfulRegistrationOnServer(short ClientNumber)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(0);
            OperationCod SuccessfulRegistrationCod = new OperationCod(ClientNumber, operationTypes, 3, 0, false, 0, 0, 0, 0, 0);
            return SuccessfulRegistrationCod;
        }
        public OperationCod CodeDisconnectingRequest(short NumberPCWhoSendRequest, short NumberDisconnectingClientPC)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(0);
            OperationCod DisconnectingRequest = new OperationCod(NumberPCWhoSendRequest, operationTypes, 4, 0, false, 0, 0, 0, 0, 0);
            DisconnectingRequest.AdditionalParam_NumRemotePC = NumberDisconnectingClientPC;
            return DisconnectingRequest;
        }
        #endregion

        #region Файловый менеджер (1)
        public OperationCod Code_InformationAboutLogicalDrives_AndRootDirectoryObjects(short NumberPC)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(1);
            operationTypes.Add(1);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 0, 0, false, 0, 0, 0, 0, 0);
            return Code;
        }
        internal OperationCod Code_Chain_InformationAboutLogicalDrives_AndRootDirectoryObjects(short NumberPC, short NumberLinkedPC, short ChainPartNumber, short TotalPartsInChain)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(1);
            operationTypes.Add(0);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 0, -1, true, NumberLinkedPC, ChainPartNumber, TotalPartsInChain, 0, 0);
            Code.AdditionalParam_NumRemotePC = NumberLinkedPC;
            return Code;
        }
        public OperationCod Code_InformationAboutLogicalDrives_AndСustomDirectoryObjects(short NumberPC, string DirPath)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(1);
            operationTypes.Add(1);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 1, 0, false, 0, 0, 0, 0, 0);
            Code.AdditionalParam_DirPath = DirPath;
            return Code;
        }
        public OperationCod CodeWorkWithTheFileManagerRequest(short NumberPCWhoSendRequest, short NumberPCWhoSendFsInfo)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(1);
            OperationCod Code = new OperationCod(NumberPCWhoSendRequest, operationTypes, 2, 0, false, 0, 0, 0, 0, 0);
            Code.AdditionalParam_NumRemotePC = NumberPCWhoSendFsInfo;
            return Code;
        }
        public OperationCod DeleteFile_Code(short NumberPC, string path)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(1);
            operationTypes.Add(1);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 3, 0, false, 0, 0, 0, 0, 0);
            Code.AdditionalParam_DirPath = path;
            return Code;
        }
        public OperationCod LaunchFile_Code(short NumberPC, string path)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(1);
            operationTypes.Add(1);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 4, 0, false, 0, 0, 0, 0, 0);
            Code.AdditionalParam_DirPath = path;
            return Code;
        }
        #endregion

        #region Трансляция экрана (2)
        public OperationCod CodeWork_RemoteScreen_Request(short NumberPCWhoSendRequest)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(2);
            OperationCod Code = new OperationCod(NumberPCWhoSendRequest, operationTypes, 0, 0, false, 0, 0, 0, 0, 0);
            return Code;
        }
        internal OperationCod Code_Chain_StartRemoteScreen(short NumberPC, short NumberLinkedPC, short ChainPartNumber, short TotalPartsInChain)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(2);
            operationTypes.Add(0);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 1, -1, true, NumberLinkedPC, ChainPartNumber, TotalPartsInChain, 0, 0);
            Code.AdditionalParam_NumRemotePC = NumberLinkedPC;
            return Code;
        }
        public OperationCod Code_StartRemoteScreen(short NumberPC)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(2);
            operationTypes.Add(2);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 1, 0, false, 0, 0, 0, 0, 0);
            return Code;
        }
        public OperationCod Code_StopRemoteScreen(short NumberPC)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(2);
            operationTypes.Add(2);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 2, 0, false, 0, 0, 0, 0, 0);
            return Code;
        }
        #endregion

        #region Управление Клавиатура и мышь (3)

        #endregion

        #region Передача файлов (4)
        public OperationCod Code_SendFile(short NumberPC, string FileReadPath, string FileWritePath)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(4);
            operationTypes.Add(4);
            OperationCod Code = new OperationCod(NumberPC, operationTypes, 0, 0, false, 0, 0, 0, 0, 0);
            Code.AdditionalParam_DirPath = FileReadPath;
            Code.AdditionalParam_DirPath2 = FileWritePath;
            return Code;
        }
        #endregion

        #region  Текстовые сообщения(5)
        public OperationCod CodeWork_TextChat_Request(short NumberPCWhoSendRequest)
        {
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(5);
            OperationCod Code = new OperationCod(NumberPCWhoSendRequest, operationTypes, 0, 0, false, 0, 0, 0, 0, 0);
            return Code;
        }
        #endregion
        #endregion
    }
}
