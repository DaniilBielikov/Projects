using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Core
{
    public interface ILogicalDisksProcessing
    {
        event EventHandler TickToProgressBar;
        event EventHandler ResetProgressBar;
        Int32 SG_NumPlusToProgressBar
        { get; set; }
        List<Tuple<string, string>> GetNamespace(string _AbsolutNameFile);
        Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> FindAllObjInFat32Directory(LogicalDisks LD, Int32 NumFistKlasteraDir);
        Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, Int32> FindAllObjInNextFat32Directory(LogicalDisks LD, Int32 NumFistKlasteraCurrentDir, string NameNextDir);
        Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, Int32> FindAllObjInFat32DirectoryPath(LogicalDisks LD, string pute);
        Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, List<Int32>> FindAllFilesWithSlovoInCurrentFat32Directory(LogicalDisks LD, Int32 NumFistKlasteraCurrentDir, string Slovo, Int32 Encod);
        List<Int32> FindAllFilesWithSlovoInCurrentNTFSDirectory(List<Int32> TypeObj, List<string> NameObj, string Slovo, string CurrentPathDir);
    }
    public class LogicalDisksProcessing : LogicalDisks, ILogicalDisksProcessing
    {
        static Encoding uni = Encoding.Unicode;
        public event EventHandler TickToProgressBar;//событие для прогрессбара плюс тик
        public event EventHandler ResetProgressBar;//событие для прогрессбара сброс
        private Int32 NumPlusToProgressBar;

        #region WinApi
        [DllImport("kernel32.dll")]
        static extern bool SetFilePointerEx(IntPtr hFile, UInt64 liDistanceToMove, IntPtr lpNewFilePointer, uint dwMoveMethod);

        [DllImport("kernel32.dll")]
        static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, [Out] int lpNumberOfBytesRead, int lpOverlapped);

        #endregion

        #region Геттеры и с Сеттеры
        public Int32 SG_NumPlusToProgressBar
        {
            get { return NumPlusToProgressBar; }
            set { NumPlusToProgressBar = value; }
        }

        #endregion

        public List<Tuple<string, string>> GetNamespace(string _AbsolutNameFile)
        {
            List<Tuple<string, string>> FileNamespace = new List<Tuple<string, string>>();
            int flg = 0;
            int[] ForbiddenSymbols = { 0x01, 0x02, 0x03, 0x04, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11,
                                           0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x20, 0x22, 0x2A,
                                            0x2B, 0x2C, /*0x2E,*/ 0x2F, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
                                             0x5B, 0x5C, 0x5D, 0x7C};

            int[] ForbiddenSymbolsLong = { 0x2F, 0x3A, 0x2A, 0x3F, 0x3C, 0x3E, 0x7C, 0xAB, 0xBB };

            //c:\q.q\q.qwe\qwertyui.q\qwertyui.qwe\qw.q.wer\q.w.q\qqwwrtrtr.ewetythtrr.rewewergt\eggbf!/? wdfdgnbdfsfdf\wegfdvefvgbrttyjhgfrettmjghfgtythttgtt.trynrbrtrtyh.grtghgtg
            //C:\qwe.q\qwe.qw\qwe.qwe\qwe.qwer\qwe.\.\.q\.qw\.qwe\.qwer\qwer
            //C:\qwe.q\qwe.qw\qwe.qwe\qwe.qwer\qwe.\.\.q\.qw\.qwe\.qwer\qwer\qwertyui\qwertyuiop.q\q\qwertyuio\ ffgngfdsdfghjhgf

            //--Начало проверка первой компоненты пути
            string[] words = _AbsolutNameFile.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (words[0].Length == 2 && (Convert.ToInt32(words[0][0]) >= 65 && Convert.ToInt32(words[0][0]) <= 90 || Convert.ToInt32(words[0][0]) >= 97 && Convert.ToInt32(words[0][0]) <= 122) && words[0][1] == ':')
            {
                if (Convert.ToInt32(words[0][0]) >= 97 && Convert.ToInt32(words[0][0]) <= 122)
                {
                    FileNamespace.Add(Tuple.Create<string, string>(Convert.ToString(Convert.ToChar(Convert.ToInt32(words[0][0]) - 32)) + words[0][1] + '\\', "Good"));
                }
                else
                {
                    FileNamespace.Add(Tuple.Create<string, string>(words[0] + '\\', "Good"));
                }
            }
            else
            {
                FileNamespace.Add(Tuple.Create<string, string>("Error!", "Bad"));
            }
            //--Конец проверка первой компоненты пути

            int ffgg = 0;
            ////-- Начало проверка остальных компонент пути.
            for (int p = 1; p < words.Length; p++)
            {
                flg = 0;
                ffgg = 0;

                for (int gg = 0; gg < words[p].Length; gg++)
                {
                    //Console.WriteLine(Convert.ToInt32(Convert.ToChar(words[p][gg])));
                    if (Convert.ToInt32(Convert.ToChar(words[p][gg])) >= 0x0400 && Convert.ToInt32(Convert.ToChar(words[p][gg])) <= 0x04FF)
                    {
                        ffgg = 1;
                        break;
                    }
                }

                int Tochka = words[p].IndexOf(Convert.ToChar(0x2E));
                int LengthSStr = words[p].Length;


                //если короткое имя
                if (ffgg == 0 && LengthSStr > 0 && LengthSStr <= 12 && Tochka == words[p].LastIndexOf(Convert.ToChar(0x2E))
                    && (((LengthSStr - Tochka) >= 1 && (LengthSStr - Tochka) <= 4
                    && Tochka >= 0 && Tochka <= 8)
                    || (LengthSStr > 0 && LengthSStr <= 8 && Tochka == -1)))
                {
                    //проверка на недопустимые символы
                    for (int ki = 0; ki < ForbiddenSymbols.Length; ki++)
                    {
                        if (words[p].IndexOf(Convert.ToChar(ForbiddenSymbols[ki])) != -1)
                        {
                            FileNamespace.Add(Tuple.Create<string, string>("Недопустимый символ:\"" + Convert.ToChar(ForbiddenSymbols[ki]) + "\"! Error! :" + words[p], "Bad"));
                            flg = 1;
                            break;
                        }
                    }
                    //если недопустимые символы не найдены
                    if (flg == 0)
                    {
                        //короткое имя не может состоять только из точки
                        if (words[p].IndexOf(Convert.ToChar(0x2E)) == 0 && words[p].LastIndexOf(Convert.ToChar(0x2E)) == 0 && words[p].Length == 1)
                        {
                            FileNamespace.Add(Tuple.Create<string, string>("Имя не может состоять только из :\"" + words[p] + "\"", "Bad"));
                        }
                        else
                        {
                            //дописываю пробелы в короткое имя если нужно
                            int InOf = words[p].IndexOf(Convert.ToChar(0x2E));
                            int Length = words[p].Length;
                            string result = words[p];

                            //дописываю пробелы в короткое имя если точка отсутствует
                            if (InOf == -1)
                            {
                                for (int i = Length; i < 12; i++)
                                {
                                    if (i == 8)
                                    {
                                        result = result + ".";
                                    }
                                    else
                                    {
                                        result = result + " ";
                                    }
                                }
                            }
                            //дописываю пробелы в короткое имя если точка присутствует
                            else
                            {
                                string str = words[p].Substring(InOf, (Length - InOf));
                                result = result.Remove(InOf, str.Length);
                                for (int i = InOf; i < 8; i++)
                                {
                                    result = result + " ";
                                }
                                for (int i = str.Length; i <= 3; i++)
                                {
                                    str = str + " ";
                                }
                                result = result + str;

                            }
                            result = result.ToUpper(); //перевод строки в верхний регистр
                            FileNamespace.Add(Tuple.Create<string, string>(result, "Short"));
                        }
                    }

                }
                //если длинное мия
                else
                {
                    //проверка на недопустимые символы
                    for (int ki = 0; ki < ForbiddenSymbolsLong.Length; ki++)
                    {
                        if (words[p].IndexOf(Convert.ToChar(ForbiddenSymbolsLong[ki])) != -1)
                        {
                            FileNamespace.Add(Tuple.Create<string, string>("Недопустимый символ:\"" + Convert.ToChar(ForbiddenSymbolsLong[ki]) + "\"! Error! :" + words[p], "Bad"));
                            flg = 1;
                            break;
                        }
                    }

                    //проверка на имя не может состоять только из пробелов и точек
                    int kol = 0;
                    for (int i = 0; i < words[p].Length; i++)
                    {
                        if (words[p][i] == Convert.ToChar(0x20) || words[p][i] == Convert.ToChar(0x2E))
                        {
                            kol++;
                        }
                    }

                    //проверка на длину длинного имени и не только
                    if (words[p].Length > 0 && words[p].Length <= 255 && flg == 0 && kol != words[p].Length && words[p][0] != Convert.ToChar(0x20))
                    {
                        FileNamespace.Add(Tuple.Create<string, string>(words[p], "Long"));
                    }
                    else
                    {
                        if (kol == words[p].Length)
                        {
                            FileNamespace.Add(Tuple.Create<string, string>("Длинное имя не может состоять только из точек и/или пробелов :" + words[p], "Bad"));
                        }
                        else
                        {
                            if (words[p].Length > 255)
                            {
                                FileNamespace.Add(Tuple.Create<string, string>("Длинное имя не может превышать 255 символов! :" + words[p], "Bad"));
                            }
                            else
                            {
                                if (words[p][0] == Convert.ToChar(0x20))
                                {
                                    FileNamespace.Add(Tuple.Create<string, string>("Длинное имя не может начинаться с пробела! :" + words[p], "Bad"));
                                }
                            }
                        }
                    }
                }
            }

            return FileNamespace;
        }

        public Tuple<Int32, List<string>> GetKolvoDeskriptorovAndSybolsNameInDeskriptors(string Name, string Type)
        {

            List<string> SybolsNameInDeskriptors = new List<string>();
            Int32 KolvoDeskriptorov = 0;

            if (Type == "Short")
            {
                Name = Name.Remove(8, 1);
                SybolsNameInDeskriptors.Add(Name);
                KolvoDeskriptorov = 1;
            }
            else if (Type == "Long")
            {
                KolvoDeskriptorov = (Int32)Math.Ceiling((Double)Name.Length / 13);
                int smesh = 13;

                for (int i = 1; i < KolvoDeskriptorov + 1; i++)
                {
                    if (Name.Length < 13)
                    {
                        smesh = Name.Length;
                    }

                    SybolsNameInDeskriptors.Add(Name.Substring(0, smesh));
                    Name = Name.Remove(0, smesh);
                }
                KolvoDeskriptorov++;
            }


            return Tuple.Create(KolvoDeskriptorov, SybolsNameInDeskriptors);
        }

        public List<UInt32> FindFileFat(Tuple<Int32, List<string>> KolvoDesk_ListStringDesk, LogicalDisks LD)
        {
            List<UInt32> ResListNumbersOfKlasters = new List<UInt32>();

            UInt64 dist = LD.SG_AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            int fl = 0;
            byte[] mas = null;
            UInt64 AdrRoot2 = 0, AdrRoot1 = 0;
            uint RazmerKlsteraByte = 0, NumOfFistKlasterObj = 0;
            UInt64 AdrFatObl = 0;
            UInt32 RazmerFatObl = 0;
            UInt32 NextNumKlastFat = 0;
            byte[] masFatObl = null;
            UInt16 NumberActiveFatTable = 0;
            Int16 SizeElZepochkiKlasterov = 0;
            int ActiveTableFat = 0;


            if (LD.SG_TypeFileSyst == "NTFS")
            {
                ResListNumbersOfKlasters.Add(0);
            }

            else if (LD.SG_TypeFileSyst == "FAT32")
            {
                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, buf, 512, nRead, 0);

                AdrRoot2 = LD.SG_AdrNuch + (UInt64)(((BitConverter.ToInt16(buf, 44) - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;
                RazmerKlsteraByte = Convert.ToUInt32(BitConverter.ToInt16(buf, 11) * buf[13]);


                AdrFatObl = Convert.ToUInt64((BitConverter.ToInt16(buf, 14) * 512)); //размер резерва/начало фат области

                RazmerFatObl = Convert.ToUInt32((BitConverter.ToInt32(buf, 36) * buf[16] * 512));  //размер фат области

                NumberActiveFatTable = BitConverter.ToUInt16(buf, 40);

                ActiveTableFat = NumberActiveFatTable & 0x000f; //дописать

                masFatObl = new byte[RazmerFatObl];  //масив байт размером в размер фат области

                mas = new byte[RazmerKlsteraByte];

                SetFilePointerEx(_Handle, AdrRoot2, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, mas, RazmerKlsteraByte, nRead, 0);

                fl = 1;
                SizeElZepochkiKlasterov = 4;
            }

            if (fl == 1) //Если Fat16/32
            {
                int i = 0;
                do
                {
                    if (mas[i] != 0xE5 && mas[i + 11] != 0x10) //обработка файлов
                    {
                        if (KolvoDesk_ListStringDesk.Item1 == 1) //поиск если 1 дескриптор (короткое имя)
                        {
                            if (mas[i + 11] == 0x20) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x20
                            {
                                string ReadStrName = "";
                                for (int y = 0; y < 11; y++)
                                {
                                    ReadStrName = ReadStrName + Convert.ToString(Convert.ToChar(mas[i + y]));
                                }

                                if (ReadStrName == KolvoDesk_ListStringDesk.Item2[0])
                                {
                                    NumOfFistKlasterObj = BitConverter.ToUInt16(mas, i + 20);
                                    NumOfFistKlasterObj = NumOfFistKlasterObj << 16;
                                    NumOfFistKlasterObj += BitConverter.ToUInt16(mas, i + 26);
                                    ResListNumbersOfKlasters.Add(NumOfFistKlasterObj);

                                }
                            }
                        }

                        else if (KolvoDesk_ListStringDesk.Item1 > 1) //поиск если более 1го дескриптора (длинное имя)
                        {
                            if (mas[i + 11] == 0x0F && mas[i] == (KolvoDesk_ListStringDesk.Item1 - 1) + 0x40) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x0F
                            {
                                string ReadStrName = "";
                                int flgEnd = 0;
                                byte[] buff = new byte[2];

                                for (int u = KolvoDesk_ListStringDesk.Item2.Count - 1; u > -1; u--)
                                {
                                    for (int y = 1; y < 11; y += 2)
                                    {

                                        buff[0] = mas[i + y];
                                        buff[1] = mas[i + y + 1];
                                        ReadStrName = ReadStrName + uni.GetString(buff);

                                    }
                                    for (int y = 14; y < 26; y += 2)
                                    {

                                        buff[0] = mas[i + y];
                                        buff[1] = mas[i + y + 1];
                                        ReadStrName = ReadStrName + uni.GetString(buff);

                                    }
                                    for (int y = 28; y < 32; y += 2)
                                    {

                                        buff[0] = mas[i + y];
                                        buff[1] = mas[i + y + 1];
                                        ReadStrName = ReadStrName + uni.GetString(buff);

                                    }

                                    buff[0] = 0x00;
                                    buff[1] = 0x00;

                                    if (KolvoDesk_ListStringDesk.Item2[KolvoDesk_ListStringDesk.Item2.Count - 1].Length != 13 && ReadStrName[KolvoDesk_ListStringDesk.Item2[KolvoDesk_ListStringDesk.Item2.Count - 1].Length] == Convert.ToChar(uni.GetString(buff)))
                                    {
                                        ReadStrName = ReadStrName.Remove(KolvoDesk_ListStringDesk.Item2[KolvoDesk_ListStringDesk.Item2.Count - 1].Length);
                                    }
                                    if (KolvoDesk_ListStringDesk.Item2[u] == ReadStrName)
                                    {
                                        i = i + 32;
                                        ReadStrName = ReadStrName.Remove(0);
                                    }
                                    else
                                    {
                                        flgEnd = 1;
                                        break;
                                    }
                                }
                                if (flgEnd == 0) //просмотр базового дескриптора длинного имени
                                {
                                    NumOfFistKlasterObj = BitConverter.ToUInt16(mas, i + 20);
                                    NumOfFistKlasterObj = NumOfFistKlasterObj << 16;
                                    NumOfFistKlasterObj += BitConverter.ToUInt16(mas, i + 26);
                                    ResListNumbersOfKlasters.Add(NumOfFistKlasterObj);
                                }

                            }
                        }
                    }


                    i = i + 32;
                }
                while (mas[i] != 0 && ResListNumbersOfKlasters.Count != 1);
            }

            if (ResListNumbersOfKlasters.Count != 0 /*||ResListNumbersOfKlasters[0] != 0*/) //если объект найден, поиск всей цепочки кластеров
            {
                SetFilePointerEx(_Handle, AdrFatObl + LD.SG_AdrNuch, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, masFatObl, RazmerFatObl, nRead, 0);
                UInt32 NumKlastera = ResListNumbersOfKlasters[0];

                do
                {
                    if (SizeElZepochkiKlasterov == 4)
                    {
                        NextNumKlastFat = 0;
                        NextNumKlastFat = masFatObl[NumKlastera * SizeElZepochkiKlasterov + 3];
                        NextNumKlastFat = NextNumKlastFat << 28;
                        NextNumKlastFat = NextNumKlastFat >> 20;
                        NextNumKlastFat += masFatObl[NumKlastera * SizeElZepochkiKlasterov + 2];
                        NextNumKlastFat = NextNumKlastFat << 8;
                        NextNumKlastFat += masFatObl[NumKlastera * SizeElZepochkiKlasterov + 1];
                        NextNumKlastFat = NextNumKlastFat << 8;
                        NextNumKlastFat += masFatObl[NumKlastera * SizeElZepochkiKlasterov];
                    }


                    if ((NextNumKlastFat >= 0x0ffffff8 && NextNumKlastFat <= 0x0fffffff && SizeElZepochkiKlasterov == 4))
                    {
                        //ResListNumbersOfKlasters.Add(NextNumKlastFat);
                        break;
                    }//Конец цепочки

                    else if ((NextNumKlastFat >= 0x00000002 && NextNumKlastFat <= 0x0fffffef && SizeElZepochkiKlasterov == 4))
                    {
                        ResListNumbersOfKlasters.Add(NextNumKlastFat);
                    }//Все норм

                    else if ((NextNumKlastFat == 0x0ffffff7 || NextNumKlastFat >= 0x0ffffff0 && NextNumKlastFat <= 0x0ffffff6 && SizeElZepochkiKlasterov == 4))
                    {
                        ResListNumbersOfKlasters.Add(1);
                    }//Ненорм (Резерв сист/бэды)

                    NumKlastera = NextNumKlastFat;

                }
                while (true);
            }

            return ResListNumbersOfKlasters;
        }

        ///<summary>
        ///
        ///</summary>
       
        public Boolean FindStringInFile(string slovo, LogicalDisks LD, List<Int32> ZepochkaKlasterov, Int32 Encod)
        {
            Boolean Nayden = false;

            UInt64 AdrNuch = LD.SG_AdrNuch;
            byte[] buf = new byte[LD.SG_KlasterSize];
            byte[] buf0 = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            uint KlasterSize = LD.SG_KlasterSize;
            UInt64 dist = 0;
            // byte[] BYTEstr = System.Text.Encoding.UTF8.GetBytes(slovo);
            byte[] BYTEstr = System.Text.Encoding.GetEncoding(Encod).GetBytes(slovo);

            int num = 0;
            // byte bb = 0;

            SetFilePointerEx(_Handle, AdrNuch, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf0, 512, nRead, 0);

            for (int i = 0; i < ZepochkaKlasterov.Count; i++)
            {
                //dist = AdrNuch + 0x53c000+(UInt64)(ZepochkaKlasterov[i] * KlasterSize);
                dist = AdrNuch + (UInt64)(((ZepochkaKlasterov[i] - 2) * buf0[13]) + BitConverter.ToInt16(buf0, 14) + buf0[16] * BitConverter.ToInt16(buf0, 36)) * 512;

                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, buf, KlasterSize, nRead, 0);



                for (int f = 0; f < KlasterSize - BYTEstr.Length; f++)
                {

                    num = f;

                    //if (num + BYTEstr.Length - 1 > KlasterSize)
                    //{
                    //    break;
                    //}
                    //ДОБАВИТЬ ОЧИСТКУ РЕЗ СПИСКА ЕСЛИ ПОДСТРОКИ В ФАЙЛЕ НЕ НАЙДЕНО
                    foreach (byte b in BYTEstr)
                    {
                        if (b != buf[num])
                        {
                            break;
                        }
                        num++;
                    }

                    if (num - f == BYTEstr.Length)
                    {
                        Nayden = true;
                        break;
                    }
                }

                if (Nayden)
                {
                    break;
                }
            }

            return Nayden;
        }
        public List<Int32> FindAllKlastersObj(LogicalDisks LD, Int32 FistKlasterObj)
        {
            List<Int32> ResListKlasterov = new List<Int32>();
            ResListKlasterov.Add(Convert.ToInt32(FistKlasterObj));

            UInt64 AdrNuch = LD.SG_AdrNuch;
            UInt64 dist = AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            UInt64 AdrRoot = LD.SG_AdrRoot;
            uint RazmerKlsteraByte = LD.SG_KlasterSize;
            UInt64 AdrFatObl = LD.SG_AdrFat;
            UInt32 RazmerFatObl = LD.SG_SizeFat;
            UInt32 NextNumKlastFat = 0;
            byte[] masFatObl = null;
            UInt64 SmesheniyeActiveTablFat = LD.SG_SmeshenieActiveTableFat;
            int fl = 0;

            masFatObl = new byte[RazmerFatObl];  //масив байт размером в размер фат области

            SetFilePointerEx(_Handle, AdrFatObl + AdrNuch + SmesheniyeActiveTablFat, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, masFatObl, RazmerFatObl, nRead, 0);

            UInt32 NumKlasteraKorenKatalog = Convert.ToUInt32(FistKlasterObj);

            do
            {
                NextNumKlastFat = 0;

                NextNumKlastFat = Convert.ToUInt32(BitConverter.ToUInt32(masFatObl, Convert.ToInt32(NumKlasteraKorenKatalog * 4)));
                NextNumKlastFat = NextNumKlastFat << 4;
                NextNumKlastFat = NextNumKlastFat >> 4;

                if ((NextNumKlastFat >= 0x0ffffff8 && NextNumKlastFat <= 0x0fffffff))
                {
                    break;
                }//Конец цепочки

                else if ((NextNumKlastFat >= 0x00000002 && NextNumKlastFat <= 0x0fffffef))
                {
                    ResListKlasterov.Add(Convert.ToInt32(NextNumKlastFat));
                }//Все норм

                else if ((NextNumKlastFat == 0x0ffffff7 || NextNumKlastFat >= 0x0ffffff0 && NextNumKlastFat <= 0x0ffffff6))
                {
                    fl = 1;
                    break;
                }//Ненорм (Резерв сист/бэды)

                NumKlasteraKorenKatalog = NextNumKlastFat;
            }
            while (true);

            if (fl == 1)
            {
                ResListKlasterov.Clear();
                ResListKlasterov.Add(-1); //Error!

            }


            return ResListKlasterov;
        }
        public Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> FindAllObjInFat32Directory(LogicalDisks LD, Int32 NumFistKlasteraDir)
        {
            List<string> ResListFiles = new List<string>();
            List<Int32> ResListNumKlastersFiles = new List<Int32>();
            List<UInt32> ResListSizeFiles = new List<UInt32>();

            List<string> ResListDir = new List<string>();
            List<Int32> ResListNumKlastersDir = new List<Int32>();
            List<UInt32> ResListSizeDir = new List<UInt32>();

            UInt64 AdrNuch = LD.SG_AdrNuch;
            UInt64 dist = AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            byte[] mas = null;
            UInt64 AdrRoot2 = LD.SG_AdrRoot;
            uint RazmerKlsteraByte = LD.SG_KlasterSize;
            int NumOfFistKlasterObj = 0;
            UInt64 AdrFatObl = LD.SG_AdrFat;
            UInt32 RazmerFatObl = LD.SG_SizeFat;
            int flg = 0;
            UInt64 SmesheniyeActiveTablFat = LD.SG_SmeshenieActiveTableFat;


            SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, nRead, 0);

            //masFatObl = new byte[RazmerFatObl];  //масив байт размером в размер фат области

            //SetFilePointerEx(_Handle, AdrFatObl + AdrNuch + SmesheniyeActiveTablFat, IntPtr.Zero, FILE_BEGIN);
            //ReadFile(_Handle, masFatObl, RazmerFatObl, nRead, 0);

            List<Int32> ListKlasrersKatalog = new List<Int32>();


            ListKlasrersKatalog = FindAllKlastersObj(LD, NumFistKlasteraDir);
            mas = new byte[RazmerKlsteraByte * ListKlasrersKatalog.Count]; //выдиление памяти для каталога

            for (int hh = 0; hh < ListKlasrersKatalog.Count; hh++)   //заполнение по кластерно содержимого каталога (дескрипторы)
            {
                dist = AdrNuch + (UInt64)(((ListKlasrersKatalog[hh] - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;
                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, mas, RazmerKlsteraByte, nRead, 0);    //чтение каталога

            }
            //ДОБАВИТЬ ПРОВЕРКУ НА ОШИБКУ ПРИ ПОИСКЕ ЦЕПОЧКИ КЛАСТЕРОВ

            int i = 0; flg = 0;
            string ReadStrName = "";
            string ReadStrNamebuff = "";

            do
            {
                if (mas[i] != 0xE5 /*&& (mas[i + 11] & 00010000) != 1*/ && mas[i] != 0x00) //обработка файлов
                {

                    if ((mas[i + 11] & 0x20) == 0x20) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x20
                    {

                        if (flg == 0) //объект с короттким именем
                        {
                            if (ReadStrName.Length > 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                            for (int y = 0; y < 11; y++)
                            {
                                ReadStrName = ReadStrName + Convert.ToString(Convert.ToChar(mas[i + y]));
                            }
                        }

                        NumOfFistKlasterObj = BitConverter.ToInt16(mas, i + 20);
                        NumOfFistKlasterObj = NumOfFistKlasterObj << 16;
                        NumOfFistKlasterObj += BitConverter.ToInt16(mas, i + 26);
                        
                        for(int jj = ReadStrName.Length; jj>0; jj--)
                        {
                            if(ReadStrName[ReadStrName.Length-1]==' ')
                            {
                                ReadStrName = ReadStrName.Remove(ReadStrName.Length - 1);
                            }
                        }

                        ResListFiles.Add(ReadStrName);
                        ResListNumKlastersFiles.Add((Int32)NumOfFistKlasterObj);
                        ResListSizeFiles.Add(BitConverter.ToUInt32(mas, i + 28));

                        if (ReadStrName.Length > 1)
                        {
                            ReadStrName = ReadStrName.Remove(0);
                        }
                        flg = 0;
                    }
                    if ((mas[i + 11] & 0x10) == 0x10) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x10
                    {

                        if (flg == 0) //объект с короттким именем
                        {
                            if (ReadStrName.Length > 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                            for (int y = 0; y < 11; y++)
                            {
                                ReadStrName = ReadStrName + Convert.ToString(Convert.ToChar(mas[i + y]));
                            }
                        }

                        NumOfFistKlasterObj = BitConverter.ToInt16(mas, i + 20);
                        NumOfFistKlasterObj = NumOfFistKlasterObj << 16;
                        NumOfFistKlasterObj += BitConverter.ToInt16(mas, i + 26);

                        for (int jj = ReadStrName.Length; jj > 0; jj--)
                        {
                            if (ReadStrName[ReadStrName.Length - 1] == ' ')
                            {
                                ReadStrName = ReadStrName.Remove(ReadStrName.Length - 1);
                            }
                        }
                        ResListDir.Add(ReadStrName);
                        ResListNumKlastersDir.Add((Int32)NumOfFistKlasterObj);
                        ResListSizeDir.Add(BitConverter.ToUInt32(mas, i + 28));

                        if (ReadStrName.Length > 1)
                        {
                            ReadStrName = ReadStrName.Remove(0);
                        }
                        flg = 0;
                    }

                    else if ((mas[i + 11] & 0x0F) == 0x0F) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x0F
                    {
                        //сложить строки дескрипторов
                        byte[] buff = new byte[2];


                        for (int y = 1; y < 11; y += 2)
                        {

                            buff[0] = mas[i + y];
                            buff[1] = mas[i + y + 1];
                            ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                        }
                        for (int y = 14; y < 26; y += 2)
                        {

                            buff[0] = mas[i + y];
                            buff[1] = mas[i + y + 1];
                            ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                        }
                        for (int y = 28; y < 32; y += 2)
                        {

                            buff[0] = mas[i + y];
                            buff[1] = mas[i + y + 1];
                            ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                        }

                        buff[0] = 0x00;
                        buff[1] = 0x00;

                        if (ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))) > 0)
                        {
                            ReadStrNamebuff = ReadStrNamebuff.Remove(ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))), ReadStrNamebuff.Length - ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))));
                        }

                        ReadStrName = ReadStrName.Insert(0, ReadStrNamebuff);
                        ReadStrNamebuff = ReadStrNamebuff.Remove(0);
                        flg = 1;



                        if (mas[i + 32] != 0xE5 && (mas[i + 11 + 32] & 0x10) == 0x10 && mas[i + 32] != 0x00 && (mas[i + 11 + 32] & 0x20) == 0x20)
                        {
                            if (ReadStrName.Length > 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                        }

                    }
                    else
                    {
                        if (ReadStrName.Length > 1)
                        {
                            ReadStrName = ReadStrName.Remove(0);
                        }
                        if (ReadStrNamebuff.Length > 1)
                        {
                            ReadStrNamebuff = ReadStrNamebuff.Remove(0);
                        }
                        flg = 0;
                    }
                }


                i = i + 32;
            }
            while (i < mas.Length);

            return Tuple.Create(ResListFiles, ResListNumKlastersFiles, ResListSizeFiles, ResListDir, ResListNumKlastersDir, ResListSizeDir);
        }

        public Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, Int32> FindAllObjInNextFat32Directory(LogicalDisks LD, Int32 NumFistKlasteraCurrentDir, string NameNextDir)
        {
            List<string> ResListFiles = new List<string>();
            List<Int32> ResListNumKlastersFiles = new List<Int32>();
            List<UInt32> ResListSizeFiles = new List<UInt32>();

            List<string> ResListDir = new List<string>();
            List<Int32> ResListNumKlastersDir = new List<Int32>();
            List<UInt32> ResListSizeDir = new List<UInt32>();

            Tuple<List<string>, List<Int32>,List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res = Tuple.Create(ResListFiles, ResListNumKlastersFiles, ResListSizeFiles, ResListDir, ResListNumKlastersDir, ResListSizeDir);

            Int32 NumOfFistKlasteraNextKatalog = 0;

            string pute = "A:\\" + NameNextDir; //приписую "A:\\", что бы была просто первая компонента пути для функции GetNamespace, при дальнейшем рассмотрении она не учитывается

            UInt64 AdrNuch = LD.SG_AdrNuch;
            UInt64 dist = AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            //int fl = 0;
            byte[] mas = null;
            UInt64 AdrRoot2 = LD.SG_AdrRoot;
            uint RazmerKlsteraByte = LD.SG_KlasterSize;
            int NumOfFistKlasterObj = 0;
            UInt64 AdrFatObl = LD.SG_AdrFat;
            UInt32 RazmerFatObl = LD.SG_SizeFat;
            byte[] masFatObl = null;
            int flg = 0;
            UInt64 SmesheniyeActiveTablFat = 0;

            List<Tuple<string, string>> Namespace;
            Namespace = GetNamespace(pute);


            SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, nRead, 0);

            //masFatObl = new byte[RazmerFatObl];  //масив байт размером в размер фат области

            //SetFilePointerEx(_Handle, AdrFatObl + AdrNuch + SmesheniyeActiveTablFat, IntPtr.Zero, FILE_BEGIN);
            //ReadFile(_Handle, masFatObl, RazmerFatObl, nRead, 0);

            List<Int32> ListKlasrersKatalog = new List<Int32>();

            //   Namespace = GetNamespace(pute);


            ListKlasrersKatalog = FindAllKlastersObj(LD, NumFistKlasteraCurrentDir);
            mas = new byte[RazmerKlsteraByte * ListKlasrersKatalog.Count]; //выдиление памяти для каталога

            for (int hh = 0; hh < ListKlasrersKatalog.Count; hh++)   //заполнение по кластерно содержимого каталога (дескрипторы)
            {
                dist = AdrNuch + (UInt64)(((ListKlasrersKatalog[hh] - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;
                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, mas, RazmerKlsteraByte, nRead, 0);    //чтение каталога

            }
            //ДОБАВИТЬ ПРОВЕРКУ НА ОШИБКУ ПРИ ПОИСКЕ ЦЕПОЧКИ КЛАСТЕРОВ

            //for (int rt = 1; rt < Namespace.Count; rt++) //Поиск конечного каталога. Проход по всем элементам пути.
            //{

            int i = 0; flg = 0;
            string ReadStrName = "";
            string ReadStrNamebuff = "";

            do
            {
                if (mas[i] != 0xE5 /*&& (mas[i + 11] & 00100000) != 1*/ && mas[i] != 0x00) //обработка каталогов
                {

                    if ((mas[i + 11] & 0x10) == 0x10 /*& 000010000)==1*/) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x10
                    {

                        if (flg == 0) //дериктория с короттким именем
                        {
                            if (ReadStrName.Length > 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                            for (int y = 0; y < 11; y++)
                            {
                                ReadStrName = ReadStrName + Convert.ToString(Convert.ToChar(mas[i + y]));
                            }
                        }

                        string str = Namespace[1].Item1;
                        if (Namespace[1].Item2 == "Short")
                        {
                            str = Namespace[1].Item1.Remove(8, 1);
                        }

                        if (ReadStrName == str) // если следующая директория найдена
                        {
                            NumOfFistKlasterObj = BitConverter.ToInt16(mas, i + 20);
                            NumOfFistKlasterObj = NumOfFistKlasterObj << 16;
                            NumOfFistKlasterObj += BitConverter.ToInt16(mas, i + 26);

                            ResListFiles.Add(ReadStrName);

                            //ResListNumbersOfKlastersAllFindFiles.Add(new List<Int32>());
                            NumOfFistKlasteraNextKatalog = (Int32)NumOfFistKlasterObj;

                            ListKlasrersKatalog.Clear();
                            ListKlasrersKatalog = FindAllKlastersObj(LD, NumOfFistKlasterObj);
                            mas = new byte[RazmerKlsteraByte * ListKlasrersKatalog.Count]; //выдиление памяти для каталога

                            for (int hh = 0; hh < ListKlasrersKatalog.Count; hh++)   //заполнение по кластерно содержимого каталога (дескрипторы)
                            {
                                dist = AdrNuch + (UInt64)(((ListKlasrersKatalog[hh] - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;
                                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                                ReadFile(_Handle, mas, RazmerKlsteraByte, nRead, 0);    //чтение каталога

                            }
                            break;
                        }
                        if (ReadStrName.Length > 1)
                        {
                            ReadStrName = ReadStrName.Remove(0);
                        }
                        flg = 0;
                    }

                    else if ((mas[i + 11] & 0x0F) == 0x0F) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x0F
                    {
                        //сложить строки дескрипторов
                        byte[] buff = new byte[2];


                        for (int y = 1; y < 11; y += 2)
                        {

                            buff[0] = mas[i + y];
                            buff[1] = mas[i + y + 1];
                            ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                        }
                        for (int y = 14; y < 26; y += 2)
                        {

                            buff[0] = mas[i + y];
                            buff[1] = mas[i + y + 1];
                            ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                        }
                        for (int y = 28; y < 32; y += 2)
                        {

                            buff[0] = mas[i + y];
                            buff[1] = mas[i + y + 1];
                            ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                        }

                        buff[0] = 0x00;
                        buff[1] = 0x00;

                        if (ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))) > 0)
                        {
                            ReadStrNamebuff = ReadStrNamebuff.Remove(ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))), ReadStrNamebuff.Length - ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))));
                        }

                        ReadStrName = ReadStrName.Insert(0, ReadStrNamebuff);
                        ReadStrNamebuff = ReadStrNamebuff.Remove(0);
                        flg = 1;



                        if (mas[i + 32] != 0xE5 && (mas[i + 11 + 32] & 0x10/*00010000*/) == 0x10 && mas[i + 32] != 0x00 /*&& (mas[i + 11 + 32] & 00100000) == 1*/)
                        {
                            if (ReadStrName.Length < 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                        }

                    }
                    else
                    {
                        if (ReadStrName.Length > 1)
                        {
                            ReadStrName = ReadStrName.Remove(0);
                        }
                        if (ReadStrNamebuff.Length > 1)
                        {
                            ReadStrNamebuff = ReadStrNamebuff.Remove(0);
                        }
                        flg = 0;
                    }
                }


                i = i + 32;
            }
            while (i < mas.Length);
            //}

            // }

            Res = FindAllObjInFat32Directory(LD, NumOfFistKlasteraNextKatalog);

            return Tuple.Create(Res.Item1, Res.Item2, Res.Item3, Res.Item4, Res.Item5, Res.Item6, NumOfFistKlasteraNextKatalog);
        }

        public Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, Int32> FindAllObjInFat32DirectoryPath(LogicalDisks LD, string pute)
        {
            List<string> ResListFiles = new List<string>();
            List<Int32> ResListNumKlastersFiles = new List<Int32>();
            List<UInt32> ResListSizeFiles = new List<UInt32>();

            List<string> ResListDir = new List<string>();
            List<Int32> ResListNumKlastersDir = new List<Int32>();
            List<UInt32> ResListSizeDir = new List<UInt32>();

            Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res = Tuple.Create(ResListFiles, ResListNumKlastersFiles, ResListSizeFiles, ResListDir, ResListNumKlastersDir, ResListSizeDir);

            Int32 NumOfFistKlasteraNextKatalog =LD.SG_FirstKlasterKornevohoKataloga;

            UInt64 AdrNuch = LD.SG_AdrNuch;
            UInt64 dist = AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            //int fl = 0;
            byte[] mas = null;
            UInt64 AdrRoot2 = LD.SG_AdrRoot;
            uint RazmerKlsteraByte = LD.SG_KlasterSize;
            int NumOfFistKlasterObj = 0;
            UInt64 AdrFatObl = LD.SG_AdrFat;
            UInt32 RazmerFatObl = LD.SG_SizeFat;
            byte[] masFatObl = null;
            int flg = 0;
            UInt64 SmesheniyeActiveTablFat = 0;

            List<Tuple<string, string>> Namespace;
            Namespace = GetNamespace(pute);


            SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, nRead, 0);

            //masFatObl = new byte[RazmerFatObl];  //масив байт размером в размер фат области

            //SetFilePointerEx(_Handle, AdrFatObl + AdrNuch + SmesheniyeActiveTablFat, IntPtr.Zero, FILE_BEGIN);
            //ReadFile(_Handle, masFatObl, RazmerFatObl, nRead, 0);

            List<Int32> ListKlasrersKatalog = new List<Int32>();

            //   Namespace = GetNamespace(pute);


            ListKlasrersKatalog = FindAllKlastersObj(LD, LD.SG_FirstKlasterKornevohoKataloga);
            mas = new byte[RazmerKlsteraByte * ListKlasrersKatalog.Count]; //выдиление памяти для каталога

            for (int hh = 0; hh < ListKlasrersKatalog.Count; hh++)   //заполнение по кластерно содержимого каталога (дескрипторы)
            {
                dist = AdrNuch + (UInt64)(((ListKlasrersKatalog[hh] - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;
                SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, mas, RazmerKlsteraByte, nRead, 0);    //чтение каталога

            }
            //ДОБАВИТЬ ПРОВЕРКУ НА ОШИБКУ ПРИ ПОИСКЕ ЦЕПОЧКИ КЛАСТЕРОВ

            for (int rt = 1; rt < Namespace.Count; rt++) //Поиск конечного каталога. Проход по всем элементам пути.
            {

                int i = 0; flg = 0;
                string ReadStrName = "";
                string ReadStrNamebuff = "";

                do
                {
                    if (mas[i] != 0xE5 /*&& (mas[i + 11] & 00100000) != 1*/ && mas[i] != 0x00) //обработка каталогов
                    {

                        if ((mas[i + 11] & 0x10) == 0x10 /*& 000010000)==1*/) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x10
                        {

                            if (flg == 0) //дериктория с короттким именем
                            {
                                if (ReadStrName.Length > 1)
                                {
                                    ReadStrName = ReadStrName.Remove(0);
                                }
                                for (int y = 0; y < 11; y++)
                                {
                                    ReadStrName = ReadStrName + Convert.ToString(Convert.ToChar(mas[i + y]));
                                }
                            }

                            string str = Namespace[rt].Item1;
                            if (Namespace[rt].Item2 == "Short")
                            {
                                str = Namespace[rt].Item1.Remove(8, 1);
                            }

                            if (ReadStrName == str) // если следующая директория найдена
                            {
                                NumOfFistKlasterObj = BitConverter.ToInt16(mas, i + 20);
                                NumOfFistKlasterObj = NumOfFistKlasterObj << 16;
                                NumOfFistKlasterObj += BitConverter.ToInt16(mas, i + 26);

                                ResListFiles.Add(ReadStrName);

                                //ResListNumbersOfKlastersAllFindFiles.Add(new List<Int32>());
                                NumOfFistKlasteraNextKatalog = (Int32)NumOfFistKlasterObj;

                                ListKlasrersKatalog.Clear();
                                ListKlasrersKatalog = FindAllKlastersObj(LD, NumOfFistKlasterObj);
                                mas = new byte[RazmerKlsteraByte * ListKlasrersKatalog.Count]; //выдиление памяти для каталога

                                for (int hh = 0; hh < ListKlasrersKatalog.Count; hh++)   //заполнение по кластерно содержимого каталога (дескрипторы)
                                {
                                    dist = AdrNuch + (UInt64)(((ListKlasrersKatalog[hh] - 2) * buf[13]) + BitConverter.ToInt16(buf, 14) + buf[16] * BitConverter.ToInt16(buf, 36)) * 512;
                                    SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
                                    ReadFile(_Handle, mas, RazmerKlsteraByte, nRead, 0);    //чтение каталога

                                }
                                break;
                            }
                            if (ReadStrName.Length > 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                            flg = 0;
                        }

                        else if ((mas[i + 11] & 0x0F) == 0x0F) //при просмотре дескрипторов ищу дескрипторы с атрибутом 0x0F
                        {
                            //сложить строки дескрипторов
                            byte[] buff = new byte[2];


                            for (int y = 1; y < 11; y += 2)
                            {

                                buff[0] = mas[i + y];
                                buff[1] = mas[i + y + 1];
                                ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                            }
                            for (int y = 14; y < 26; y += 2)
                            {

                                buff[0] = mas[i + y];
                                buff[1] = mas[i + y + 1];
                                ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                            }
                            for (int y = 28; y < 32; y += 2)
                            {

                                buff[0] = mas[i + y];
                                buff[1] = mas[i + y + 1];
                                ReadStrNamebuff = ReadStrNamebuff + uni.GetString(buff);

                            }

                            buff[0] = 0x00;
                            buff[1] = 0x00;

                            if (ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))) > 0)
                            {
                                ReadStrNamebuff = ReadStrNamebuff.Remove(ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))), ReadStrNamebuff.Length - ReadStrNamebuff.IndexOf(Convert.ToChar(uni.GetString(buff))));
                            }

                            ReadStrName = ReadStrName.Insert(0, ReadStrNamebuff);
                            ReadStrNamebuff = ReadStrNamebuff.Remove(0);
                            flg = 1;



                            if (mas[i + 32] != 0xE5 && (mas[i + 11 + 32] & 0x10/*00010000*/) == 0x10 && mas[i + 32] != 0x00 /*&& (mas[i + 11 + 32] & 00100000) == 1*/)
                            {
                                if (ReadStrName.Length < 1)
                                {
                                    ReadStrName = ReadStrName.Remove(0);
                                }
                            }

                        }
                        else
                        {
                            if (ReadStrName.Length > 1)
                            {
                                ReadStrName = ReadStrName.Remove(0);
                            }
                            if (ReadStrNamebuff.Length > 1)
                            {
                                ReadStrNamebuff = ReadStrNamebuff.Remove(0);
                            }
                            flg = 0;
                        }
                    }


                    i = i + 32;
                }
                while (i < mas.Length);
            }

            // }

            Res = FindAllObjInFat32Directory(LD, NumOfFistKlasteraNextKatalog);

            return Tuple.Create(Res.Item1, Res.Item2, Res.Item3, Res.Item4, Res.Item5, Res.Item6, NumOfFistKlasteraNextKatalog);
        }

        public Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, List<Int32>> FindAllFilesWithSlovoInCurrentFat32Directory(LogicalDisks LD, Int32 NumFistKlasteraCurrentDir, string Slovo, Int32 Encod)
        {
            List<string> ResListFiles = new List<string>();
            List<Int32> ResListNumKlastersFiles = new List<Int32>();
            List<UInt32> ResListSizeFiles = new List<UInt32>();

            List<string> ResListDir = new List<string>();
            List<Int32> ResListNumKlastersDir = new List<Int32>();
            List<UInt32> ResListSizeDir = new List<UInt32>();

            Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res = Tuple.Create(ResListFiles, ResListNumKlastersFiles, ResListSizeFiles, ResListDir, ResListNumKlastersDir, ResListSizeDir);

            List<Int32> ResNumsOfFindFiles = new List<Int32>();

            UInt64 AdrNuch = LD.SG_AdrNuch;
            UInt64 dist = AdrNuch;
            byte[] buf = new byte[512];
            int nRead = 0;
            IntPtr _Handle = LD.SG_Handle;
            UInt64 AdrRoot2 = LD.SG_AdrRoot;
            uint RazmerKlsteraByte = LD.SG_KlasterSize;
            UInt64 AdrFatObl = LD.SG_AdrFat;
            UInt32 RazmerFatObl = LD.SG_SizeFat;
            byte[] masFatObl = null;
            UInt64 SmesheniyeActiveTablFat = LD.SG_SmeshenieActiveTableFat;

            SetFilePointerEx(_Handle, dist, IntPtr.Zero, FILE_BEGIN);
            ReadFile(_Handle, buf, 512, nRead, 0);

            List<Int32> ListKlasrersKatalog = new List<Int32>();
            Res = FindAllObjInFat32Directory(LD, NumFistKlasteraCurrentDir);

            if (Res.Item2.Count > 0)
            {
                SetFilePointerEx(_Handle, AdrFatObl + LD.SG_AdrNuch + SmesheniyeActiveTablFat, IntPtr.Zero, FILE_BEGIN);
                ReadFile(_Handle, masFatObl, RazmerFatObl, nRead, 0);

                List<Int32> ddd = new List<Int32>();

                //расчет значений для прогресс бара. зависит от количества файлов в дериктории и шкалы прогрессбара
                NumPlusToProgressBar = Convert.ToInt32(Math.Round(Convert.ToDouble(999999 / Res.Item2.Count)));
                if(NumPlusToProgressBar == 0)
                {
                    NumPlusToProgressBar = 1;
                }
                //
                for (int k = 0; k < Res.Item2.Count; k++)
                {
                    Int32 NumKlastera = Res.Item2[k];

                    ddd = FindAllKlastersObj(LD, NumKlastera);

                    if (FindStringInFile(Slovo, LD, ddd, Encod) == false) //если подстрока не найдена
                    {
                        ddd.Clear();
                        ResNumsOfFindFiles.Add(-1);
                    }
                    else
                    {
                        ResNumsOfFindFiles.Add(1);
                        //ResNumsOfFindFiles.Add(k);
                    }

                    if (TickToProgressBar != null) TickToProgressBar(this, EventArgs.Empty); //генерю событие чтобы прогрессбар увеличил значение
                }
                if (ResNumsOfFindFiles.Count == 0)
                {
                    ResNumsOfFindFiles.Add(-1);
                }
            }
            if (ResetProgressBar != null) ResetProgressBar(this, EventArgs.Empty); //генерю событие чтобы прогрессбар сбросил значение

            return Tuple.Create(Res.Item1, Res.Item2, Res.Item3, Res.Item4, Res.Item5, Res.Item6, ResNumsOfFindFiles);
        }

        public List<Int32> FindAllFilesWithSlovoInCurrentNTFSDirectory (List<Int32> TypeObj, List<string> NameObj, string Slovo, string CurrentPathDir )
        {
            List<Int32> HighlightItems = new List<int>();
            string line;
            int flg = 0;

            //расчет значений для прогресс бара. зависит от количества файлов в дериктории и шкалы прогрессбара
            int countFiles = 1;
            for (int hp=0; hp < TypeObj.Count; hp++)
            {
                if(TypeObj[hp]==1)
                { countFiles++; }
            }
            NumPlusToProgressBar = Convert.ToInt32(Math.Round(Convert.ToDouble(999999 / countFiles)));
            if (NumPlusToProgressBar == 0)
            {
                NumPlusToProgressBar = 1;
            }

            for (int hf = 0; hf < TypeObj.Count; hf++)
            {
                flg = 0;
                System.IO.StreamReader file = null;

                if (TypeObj[hf] == 1)
                {
                    if (TickToProgressBar != null) TickToProgressBar(this, EventArgs.Empty); //генерю событие чтобы прогрессбар увеличил значение
                    try
                    {
                        file = new System.IO.StreamReader(CurrentPathDir + "\\" + NameObj[hf]);
                    }
                    catch
                    {

                        //throw;
                        HighlightItems.Add(-1);
                        continue;
                    }
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Contains(Slovo))
                        {
                            HighlightItems.Add(1);
                            flg = 1;
                            break;
                        }


                    }
                    if (flg == 0)
                    {
                        HighlightItems.Add(-1);
                    }
                }
            }
            if (ResetProgressBar != null) ResetProgressBar(this, EventArgs.Empty); //генерю событие чтобы прогрессбар сбросил значение
            return HighlightItems;
        }
    }
}
