using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryRemotePC
{
    /// <summary>
    /// Содержит набор заготовок используемых команд
    /// </summary>
    public class Codes
    {
        public Codes ()
        { }

        #region Сеть (0)
        internal List<OperationCod> CodeWaitAndInitiateAdditionalConnection(short ClientNumber, short ChainPartNumber, ushort AdditionalOperationType, short TotalPartsInChain)
        {
            List<OperationCod> Codes = new List<OperationCod>();
            List<ushort> operationTypes = new List<ushort>();
            operationTypes.Add(0);
            operationTypes.Add(AdditionalOperationType);
            short OpStatus = 0;
            if (ChainPartNumber > 0)
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
    }
}
