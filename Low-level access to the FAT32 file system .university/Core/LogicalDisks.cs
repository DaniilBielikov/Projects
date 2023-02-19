using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Management;

namespace Core
{
    public interface ILogicalDisks
    {
        List<Tuple<string, string, UInt64>> Get_List_NameLD_TypeFileSyst_AdrNuchLD(IntPtr Handle, string TypeOfSegmentation, bool IsGood);
        Tuple<UInt32, UInt64, UInt64, UInt32, Int32, UInt64, Int32> Get_LD_Info(LogicalDisks LD);
        string SG_NameLD
        {
            set;
            get;
        }
        string SG_TypeFileSyst
        {
            set;
            get;
        }
        UInt64 SG_AdrNuch
        {
            set;
            get;
        }
        uint SG_KlasterSize
        {
            get;
            set;
        }
        UInt64 SG_AdrRoot
        {
            get;
            set;
        }
        UInt64 SG_AdrFat
        {
            get;
            set;
        }
        UInt32 SG_SizeFat
        {
            get;
            set;
        }
        Int32 SG_ActiveTableFat
        {
            get;
            set;
        }
        UInt64 SG_SmeshenieActiveTableFat
        {
            get;
            set;
        }
        Int32 SG_FirstKlasterKornevohoKataloga
        {
            get;
            set;
        }

    }
    public class LogicalDisks : StorageDevices, ILogicalDisks, IStorageDevices
    {
        #region Переменные
        public static int countLD = -1;
        private string NameLD;
        private string TypeFileSyst;
        private UInt64 AdrNuchLD;

        private uint KlasterSize;
        private UInt64 AdrRoot;
        private UInt64 AdrFat;
        private UInt32 SizeFat;
        private Int32 ActiveTableFat;
        private UInt64 SmeshenieActiveTableFat;
        private Int32 FirstKlasterKornevohoKataloga;

        private List<string> _List_disks_WMI = new List<string>();
        private List<string> S_List_DevType_WMI = new List<string>();
        private List<int> _List_DevType_WMI = new List<int>();
        private List<Tuple<string, string>> Tuple_List_GUID_NameLD_WMI = new List<Tuple<string, string>>();

        private UInt32 Data1;
        private UInt16 Data2, Data3;
        private byte[] Data4 = new byte[8];
        #endregion

        #region WinApi
        [DllImport("kernel32.dll")]
        static extern bool SetFilePointerEx(IntPtr hFile, UInt64 liDistanceToMove, IntPtr lpNewFilePointer, uint dwMoveMethod);

        [DllImport("kernel32.dll")]
        static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, [Out] int lpNumberOfBytesRead, int lpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern bool GetVolumeInformation(string Volume, StringBuilder VolumeName,
        uint VolumeNameSize, out uint SerialNumber, out uint SerialNumberLength,
        out uint flags, StringBuilder fs, uint fs_size);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetDiskFreeSpace(string lpRootPathName, out int lpSectorsPerCluster, out int lpBytesPerSector, out int lpNumberOfFreeClusters, out int lpTotalNumberOfClusters);
        #endregion

        #region Геттеры и с Сеттеры
        public string SG_NameLD
        {
            get { return NameLD; }
            set { NameLD = value; }
        }
        public string SG_TypeFileSyst
        {
            get { return TypeFileSyst; }
            set { TypeFileSyst = value; }
        }
        public UInt64 SG_AdrNuch
        {
            get { return AdrNuchLD; }
            set { AdrNuchLD = value; }
        }
        public uint SG_KlasterSize
        {
            get { return KlasterSize; }
            set { KlasterSize = value; }
        }
        public UInt64 SG_AdrRoot
        {
            get { return AdrRoot; }
            set { AdrRoot = value; }
        }
        public UInt64 SG_AdrFat
        {
            get { return AdrFat; }
            set { AdrFat = value; }
        }
        public UInt32 SG_SizeFat
        {
            get { return SizeFat; }
            set { SizeFat = value; }
        }
        public Int32 SG_ActiveTableFat
        {
            get { return ActiveTableFat; }
            set { ActiveTableFat = value; }
        }
        public UInt64 SG_SmeshenieActiveTableFat
        {
            get { return SmeshenieActiveTableFat; }
            set { SmeshenieActiveTableFat = value; }
        }
        public Int32 SG_FirstKlasterKornevohoKataloga
        {
            get { return FirstKlasterKornevohoKataloga; }
            set { FirstKlasterKornevohoKataloga = value; }
        }
        #endregion

        private List<Tuple<string, string>> WMI_Carry_GUID_NameLD()  //Зарос к WMI
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();

            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            ManagementScope scope = new ManagementScope(string.Format("root\\cimv2"), options);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Volume");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                string g = Convert.ToString(m["DeviceID"]);
                string n = Convert.ToString(m["DriveLetter"]);
                if (n == "")
                { n = "-1"; }

                result.Add(Tuple.Create<string, string>(g, n));
            }
            return result;
        }
        private string GetNameLDGPT(List<Tuple<string, string>> _Tuple_List_GUID_NameLD_WMI, string str0)
        {
            string resName = "Nop!Oopsee! ";
            string str1 = "0";

            for (int i = 0; i < _Tuple_List_GUID_NameLD_WMI.Count; i++)
            {
                str1 = _Tuple_List_GUID_NameLD_WMI[i].Item1;
                str1 = str1.Remove(0, 11);
                str1 = str1.Remove(8, 1);
                str1 = str1.Remove(12, 1);
                str1 = str1.Remove(16, 1);
                str1 = str1.Remove(20, 1);
                str1 = str1.Remove(32, 2);

                if (str0 == str1)
                {
                    if (_Tuple_List_GUID_NameLD_WMI[i].Item2 != "-1")
                    {
                        resName = _Tuple_List_GUID_NameLD_WMI[i].Item2 + "\\";
                        break;
                    }
                }
            }

            return resName;
        }
        private string GetNameLDMBR(string FS, byte[] buf_sect_local0, List<string> List_disks_WMI, List<int> List_DevType_WMI)
        {
            string resName = "Nop!Oopsee! ";
            int flg_kod_syst = 0;
            StringBuilder volumename = new StringBuilder(256);
            StringBuilder fstype = new StringBuilder(256);
            string name_local_disc_zaproshennoe;
            uint disk_serialINT = 0, serialNumLength, flags, serial_number = 0;

            //////////////////////////////--START--//////////////////


            if (FS == "NTFS")   //NTFS
            {
                flg_kod_syst = 1;
                serial_number = BitConverter.ToUInt32(buf_sect_local0, 72);
                // pp = BitConverter.ToUInt64(buf_sect_local0, 72);
            }

            if (FS == "FAT12" || FS == "FAT16")   //FAT12/16 
            {
                flg_kod_syst = 1;
                serial_number = BitConverter.ToUInt32(buf_sect_local0, 39);
            }

            if (FS == "FAT32")			///FAT32
			{
                flg_kod_syst = 1;
                serial_number = BitConverter.ToUInt32(buf_sect_local0, 67);
            }
            if (flg_kod_syst == 1)
            {
                for (int j = 0; j < List_disks_WMI.Count; j++)
                {

                    name_local_disc_zaproshennoe = List_disks_WMI[j];

                    name_local_disc_zaproshennoe = name_local_disc_zaproshennoe + "\\";

                    if (!GetVolumeInformation(name_local_disc_zaproshennoe, volumename, (uint)volumename.Capacity - 1, out disk_serialINT, out serialNumLength, out flags, fstype, (uint)fstype.Capacity - 1))
                    {
                        break;
                    }
                    if (serial_number == disk_serialINT)
                    {
                        if (List_DevType_WMI[j] == 12 || List_DevType_WMI[j] == 0)           //проверяю на код устройства если 12 - жесткий диск, если 11 - дисковод, если 0 - внешний накопитель
                        {
                            resName = name_local_disc_zaproshennoe;
                        }
                    }
                }
            }

            /////////////////////////////--END--////////////////////

            return resName;
        }
        private string GetTypeOfFileSyst(IntPtr _Handle, UInt64 _AdrNuchLD)
        {
            string resType = "Nop!Oopsee! ";
            byte[] buf = new byte[512];
            int _nRead = 0;
            Int64 razmer;
            razmer = 0;
            SetFilePointerEx(_Handle, _AdrNuchLD, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, _nRead, 0);

            if (BitConverter.ToInt64(buf, 3) == 0x202020205346544E) //ntfs
            {
                resType = "NTFS";
            }
            else         ///fat
            {
                if (BitConverter.ToInt16(buf, 22) == 0 && buf[13] != 0)    ////fat32
                {
                    razmer = BitConverter.ToInt32(buf, 32) - (BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt32(buf, 36));
                    razmer /= buf[13];
                    resType = "FAT32";
                }
                else if (buf[13] != 0)   ///fat12/16
                {
                    razmer = BitConverter.ToInt32(buf, 32) - (BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 22) + BitConverter.ToInt16(buf, 17));
                    razmer /= buf[13];
                    if (razmer < 4085)
                    {
                        resType = "FAT12";
                    }
                    if (razmer >= 4085 && razmer < 64525)
                    {
                        resType = "FAT16";
                    }
                }
            }

            return resType;
        }
        public List<Tuple<string, string, UInt64>> Get_List_NameLD_TypeFileSyst_AdrNuchLD(IntPtr Handle, string TypeOfSegmentation, bool IsGood)
        {
            List<Tuple<string, string, UInt64>> ResTuples = new List<Tuple<string, string, UInt64>>();

            UInt64 dist = 0, adr_sled_mbr = 0;
            byte[] buf = new byte[512];
            byte[] buf_sect_local = new byte[512];
            byte[] buf_sect_local3 = null;
            int nRead = 0;
            int num_byt = 446;
            UInt32 kod_syst = 0, otnos_adr_sect, razmer_zagolovka, yyy, KolichestvoZapisey = 0;
            int ii = 0, fls = 1;
            string TpFs = "****", NamLD = "****";
            UInt64 AdrLD = 0, lba_nach_PT, lba_nach_razdela;
            string str = null, str2 = null;

            _List_disks_WMI = WMI_Carry("Win32_LogicalDisk", "DeviceID");
            S_List_DevType_WMI = WMI_Carry("Win32_LogicalDisk", "MediaType");
            Tuple_List_GUID_NameLD_WMI = WMI_Carry_GUID_NameLD();


            for (int i = 0; i < S_List_DevType_WMI.Count; i++)
            {
                if (S_List_DevType_WMI[i] == "")
                {
                    _List_DevType_WMI.Add(0);
                }
                else
                {
                    _List_DevType_WMI.Add(Convert.ToInt32(S_List_DevType_WMI[i]));
                }
            }

            if (TypeOfSegmentation == "MBR" && Handle != (IntPtr)(-1) && IsGood == true)
            {
                SetFilePointerEx(Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(Handle, buf, 512, nRead, 0);

                ///////////////////////--НАЧАЛО--///////////////////////////////////////////////////////////////////////////////////

                fls = 0;

                for (int q = 0; q < 4; q++)
                {
                    otnos_adr_sect = BitConverter.ToUInt32(buf, num_byt + 8);
                    dist = (UInt64)otnos_adr_sect * 512;
                    AdrLD = dist;

                    SetFilePointerEx(Handle, dist, IntPtr.Zero, FILE_BEGIN);
                    ReadFile(Handle, buf_sect_local, 512, nRead, 0);

                    if (buf[num_byt + 4] == 0x05 || buf[num_byt + 4] == 0x0F)
                    {
                        fls = 1;
                        adr_sled_mbr = dist;
                    }
                    else
                    {
                        TpFs = GetTypeOfFileSyst(Handle, AdrLD);
                        NamLD = GetNameLDMBR(TpFs, buf_sect_local, _List_disks_WMI, _List_DevType_WMI);
                        ResTuples.Add(Tuple.Create<string, string, UInt64>(NamLD, TpFs, AdrLD));
                    }

                    num_byt = num_byt + 16;
                }
                if (fls == 1)
                {
                    num_byt = 446;
                    dist = adr_sled_mbr;
                    SetFilePointerEx(Handle, adr_sled_mbr, IntPtr.Zero, FILE_BEGIN);
                    ReadFile(Handle, buf_sect_local, 512, nRead, 0);
                    do
                    {
                        if (buf_sect_local[510] == 0x55 && buf_sect_local[511] == 0xaa)
                        {
                            kod_syst = buf_sect_local[num_byt + 4];
                            otnos_adr_sect = BitConverter.ToUInt32(buf_sect_local, num_byt + 8);

                            if (kod_syst != 0x05 && kod_syst != 0x0f && ii == 1)
                            {
                                break;
                            }

                            if (ii == 0)
                            {
                                dist = dist + ((UInt64)otnos_adr_sect * 512);
                                AdrLD = dist;
                                SetFilePointerEx(Handle, dist, IntPtr.Zero, FILE_BEGIN);
                                ReadFile(Handle, buf, 512, nRead, 0);

                                //////-------------------------------Добавить алгоритм определения типа файловой системы-------------------------------------                                  
                                TpFs = GetTypeOfFileSyst(Handle, AdrLD);
                                NamLD = GetNameLDMBR(TpFs, buf, _List_disks_WMI, _List_DevType_WMI);
                                ResTuples.Add(Tuple.Create<string, string, UInt64>(NamLD, TpFs, AdrLD));
                                ////////////////ИМЯ ЛОКАЛЬНЫХ ДИСКОВ
                            }
                            else
                            {
                                dist = adr_sled_mbr + ((UInt64)otnos_adr_sect * 512);
                                SetFilePointerEx(Handle, dist, IntPtr.Zero, FILE_BEGIN);
                                ReadFile(Handle, buf_sect_local, 512, nRead, 0);
                                ii = -1;
                                num_byt = 430;
                            }
                            num_byt = num_byt + 16;
                            ii++;
                        }
                    }
                    while (true);

                }
                ///////////////////////--КОНЕЦ--//////////////////////////////////////////////////////////////////////////////////
            }

            else if (TypeOfSegmentation == "GPT" && Handle != (IntPtr)(-1) && IsGood == true)
            {
                ///////////////////////--Start--/////////////////////
                SetFilePointerEx(Handle, dist, IntPtr.Zero, FILE_BEGIN); // which sector to read
                ReadFile(Handle, buf, 512, nRead, 0);  // read sector

                for (int q = 0; q < 4; q++)
                {
                    kod_syst = buf[num_byt + 4];

                    otnos_adr_sect = BitConverter.ToUInt32(buf, num_byt + 8);
                    dist = (UInt64)otnos_adr_sect * 512;
                    AdrLD = dist;

                    SetFilePointerEx(Handle, dist, IntPtr.Zero, FILE_BEGIN);
                    ReadFile(Handle, buf_sect_local, 512, nRead, 0);

                    KolichestvoZapisey = BitConverter.ToUInt32(buf_sect_local, 80);
                    KolichestvoZapisey = KolichestvoZapisey * 128;
                    buf_sect_local3 = new byte[KolichestvoZapisey];

                    razmer_zagolovka = BitConverter.ToUInt32(buf_sect_local, 12);

                    if (razmer_zagolovka >= 92 && razmer_zagolovka < 521)
                    {
                        lba_nach_PT = 0;

                        lba_nach_PT = BitConverter.ToUInt64(buf_sect_local, 72);
                        lba_nach_PT = (UInt64)lba_nach_PT * 512;

                        SetFilePointerEx(Handle, lba_nach_PT, IntPtr.Zero, FILE_BEGIN);
                        ReadFile(Handle, buf_sect_local3, /*16384*/KolichestvoZapisey, nRead, 0);
                    }

                    if (kod_syst == 0xee || kod_syst == 0xef)
                    {
                        break;
                    }
                    num_byt = num_byt + 16;
                }

                num_byt = 0;


                do
                {

                    lba_nach_razdela = 0;
                    lba_nach_razdela = BitConverter.ToUInt64(buf_sect_local3, 32 + num_byt);
                    lba_nach_razdela = (UInt64)lba_nach_razdela * 512;

                    Data1 = BitConverter.ToUInt32(buf_sect_local3, 16 + num_byt);
                    Data2 = BitConverter.ToUInt16(buf_sect_local3, 20 + num_byt);
                    Data3 = BitConverter.ToUInt16(buf_sect_local3, 22 + num_byt);

                    int fg = 24;
                    for (int d = 0; d < 8; d++)
                    {
                        Data4[d] = buf_sect_local3[fg + num_byt];
                        fg++;
                    }

                    //str2 = str2.Remove(0, 32);

                    str2 = Convert.ToString(Data1, 16);
                    if (str2.Length < 8)
                    {
                        yyy = Convert.ToUInt32(8 - str2.Length);
                        while (yyy > 0)
                        {
                            str2 = "0" + str2;
                            yyy--;
                        }
                    }
                    str = str + str2;

                    str2 = Convert.ToString(Data2, 16);
                    if (str2.Length < 4)
                    {
                        yyy = Convert.ToUInt32(4 - str2.Length);
                        while (yyy > 0)
                        {
                            str2 = "0" + str2;
                            yyy--;
                        }
                    }
                    str = str + str2;

                    str2 = Convert.ToString(Data3, 16);
                    if (str2.Length < 4)
                    {
                        yyy = Convert.ToUInt32(4 - str2.Length);
                        while (yyy > 0)
                        {
                            str2 = "0" + str2;
                            yyy--;
                        }
                    }
                    str = str + str2;

                    for (int hh = 0; hh < 8; hh++)
                    {
                        str2 = Convert.ToString(Data4[hh], 16);

                        if (str2.Length < 2)
                        {
                            yyy = Convert.ToUInt32(2 - str2.Length);
                            while (yyy > 0)
                            {
                                str2 = "0" + str2;
                                yyy--;
                            }
                        }
                        str = str + str2;
                    }

                    if (lba_nach_razdela != 0)
                    {
                        NamLD = GetNameLDGPT(Tuple_List_GUID_NameLD_WMI, str);
                        TpFs = GetTypeOfFileSyst(Handle, lba_nach_razdela);
                        //////----------------------------Добавить алгоритм определения типа файловой системы------------------------------------------
                        ResTuples.Add(Tuple.Create<string, string, UInt64>(NamLD, TpFs, lba_nach_razdela));
                        str = str.Remove(0, 32);
                        num_byt = num_byt + 128;
                    }
                    else
                    {
                        break;
                    }
                }
                while (num_byt <= KolichestvoZapisey);


                //////////////////////--End--///////////////////////
            }

            else
            {
                ResTuples.Add(Tuple.Create<string, string, UInt64>("Error Name!", "Error Type of File Syst.!", 0));
                //Error
            }

            return ResTuples;
        }

        ///<summary>
        ///Returns values:
        ///UInt32 _KlasterSize;
        ///UInt64 _AdrRoot;
        ///UInt64 _AdrFat;
        ///UInt32 _SizeFat;
        ///Int32 _ActiveTableFat;
        ///UInt64 _SmeshenieTableFat;
        ///UInt32 _FirstKlasterKornevohoKataloga;
        ///</summary>
        public Tuple<UInt32, UInt64, UInt64, UInt32, Int32, UInt64, Int32> Get_LD_Info(LogicalDisks LD)
        {
            UInt32 _KlasterSize = 0;
            UInt64 _AdrRoot = 0;
            UInt64 _AdrFat = 0;
            UInt32 _SizeFat = 0;
            Int32 _ActiveTableFat = 0;
            UInt64 _SmeshenieActiveTableFat = 0;
            Int32 _FirstKlasterKornevohoKataloga = 0;

            UInt64 AdrNuch = LD.SG_AdrNuch;
            UInt64 dist = AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            UInt16 NumberActiveFatTable = 0;

            if (LD.SG_TypeFileSyst == "FAT32")
            {
                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, buf, 512, nRead, 0);

                _AdrRoot = LD.SG_AdrNuch + (UInt64)(((BitConverter.ToInt16(buf, 44) - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;

                _KlasterSize = Convert.ToUInt32(BitConverter.ToInt16(buf, 11) * buf[13]);

                _AdrFat = Convert.ToUInt64((BitConverter.ToInt16(buf, 14) * 512)); //размер резерва/начало фат области

                _SizeFat = Convert.ToUInt32((BitConverter.ToInt32(buf, 36) * buf[16] * 512));  //размер фат области

                NumberActiveFatTable = BitConverter.ToUInt16(buf, 40);

                _ActiveTableFat = NumberActiveFatTable & 0x000f; //дописать

                _SmeshenieActiveTableFat = Convert.ToUInt64(ActiveTableFat * BitConverter.ToInt32(buf, 36));

                _FirstKlasterKornevohoKataloga = Convert.ToInt32(BitConverter.ToInt16(buf, 44));
            }
            else if(LD.SG_TypeFileSyst == "FAT12" || LD.SG_TypeFileSyst == "FAT16" || LD.SG_TypeFileSyst == "NTFS")
            {
                int SectorsPerCluster, BytesPerSector, NumberOfFreeClusters, TotalNumberOfClusters;

                GetDiskFreeSpace(LD.NameLD, out SectorsPerCluster, out BytesPerSector, out NumberOfFreeClusters, out TotalNumberOfClusters);
                _KlasterSize = Convert.ToUInt32(SectorsPerCluster * BytesPerSector);
            }

            return Tuple.Create<UInt32, UInt64, UInt64, UInt32, Int32, UInt64, Int32>(_KlasterSize, _AdrRoot, _AdrFat, _SizeFat, _ActiveTableFat, _SmeshenieActiveTableFat, _FirstKlasterKornevohoKataloga);
        }
    }
}
