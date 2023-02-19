using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Management;

namespace Core
{
    public interface IStorageDevices
    {
        List<string> WMI_Carry(string NameWMIclass, string NameProperties);
        void CloseHandleDisk(IntPtr Handle);
        IntPtr GetHandle(string _DiskID);
        bool GetIsGood(IntPtr _Handle);
        string GetTypeOfSegmentation(IntPtr _Handle);
        string SG_DiskID
        {
            get; 
            set; 
        }
        IntPtr SG_Handle
        {
            get;
            set;
        }
        string SG_NameStorageDevice
        {
            get;
            set;
        }
        string SG_TypeOfSegmentation
        {
            get;
            set;
        }
        bool SG_IsGood
        {
            get;
            set;
        }
    }
       public class StorageDevices : IStorageDevices
    {
        public static int countSD = -1;
        private string DiskID;
        private IntPtr Handle;
        private string NameStorageDevice;
        private string TypeOfSegmentation;
        private bool IsGood;

        protected const int GENERIC_READ = 0x00000001;
        protected const int FILE_SHARE_READ = 0x00000001;
        protected const int OPEN_EXISTING = 3;
        protected const int FILE_BEGIN = 0;
        protected const Int64 INVALID_HANDLE_VALUE = -1;
        protected const int FILE_SHARE_WRITE = 0x00000002;
        protected const int GENERIC_WRITE = 0x00000002;

        #region WinApi

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFile(string lpFileName, int FileAccess, int FileShare, IntPtr lpSecurityAttributes, int FileMode, int FileAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        static extern bool SetFilePointerEx(IntPtr hFile, long liDistanceToMove, IntPtr lpNewFilePointer, uint dwMoveMethod);

        [DllImport("kernel32.dll")]
        static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, [Out] int lpNumberOfBytesRead, int lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hHandle);

        #endregion

        public List<string> WMI_Carry(string NameWMIclass, string NameProperties)  //Зарос к WMI
        {
            List<string> result = new List<string>();
            //string NameWMIclass = "Win32_DiskDrive";
            //string NameProperties = "DeviceID";

            //string CompurerName = "NOTEBOOK-KONA";
            // ManagementScope scope = new ManagementScope(string.Format("\\\\" + CompurerName + "\\root\\cimv2"), options);

            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            ManagementScope scope = new ManagementScope(string.Format("root\\cimv2"), options);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM " + NameWMIclass));
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                result.Add(Convert.ToString(m[NameProperties]));
            }
            return result;
        }

        #region Геттеры и с Сеттеры
        public string SG_DiskID
        {
            get { return DiskID; }
            set { DiskID = value; }
        }
        public IntPtr SG_Handle
        {
            get { return Handle; }
            set { Handle = value; }
        }
        public string SG_NameStorageDevice
        {
            get { return NameStorageDevice; }
            set { NameStorageDevice = value; }
        }
        public string SG_TypeOfSegmentation
        {
            get { return TypeOfSegmentation; }
            set { TypeOfSegmentation = value; }
        }
        public bool SG_IsGood
        {
            get { return IsGood; }
            set { IsGood = value; }
        }
        #endregion

        public void CloseHandleDisk(IntPtr Handle)
        {
            CloseHandle(Handle);
        }
        public IntPtr GetHandle(string _DiskID)
        {
            IntPtr resHandle;

            resHandle = CreateFile(_DiskID, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            return resHandle;
        }
        public bool GetIsGood(IntPtr _Handle)
        {
            bool resIsGood;
            Int64 dist = 0;
            byte[] buf = new byte[512];
            int nRead = 0;

            SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, nRead, 0);  // read sector

            if (buf[510] == 0x55 && buf[511] == 0xaa)
            { resIsGood = true; }
            else
            { resIsGood = false; }

            return resIsGood;
        }
        public string GetTypeOfSegmentation(IntPtr _Handle)
        {
            string resTypeOfSegmentation = "MBR";

            Int64 dist = 0;
            byte[] buf = new byte[512];
            int nRead = 0;
            int num_byt = 446;

            SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, nRead, 0);
            if (buf[510] == 0x55 && buf[511] == 0xaa)
            {
                for (int q = 0; q < 4; q++)
                {
                    if ((int)(buf[num_byt + 4]) == 0xee || (int)(buf[num_byt + 4]) == 0xef)
                    {
                        resTypeOfSegmentation = "GPT";
                        break;
                    }
                    num_byt = num_byt + 16;
                }
            }
            else
            { resTypeOfSegmentation = "Error! Signature not 55AA!"; }

            return resTypeOfSegmentation;
        }

    }
}