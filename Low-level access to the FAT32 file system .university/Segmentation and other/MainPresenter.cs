using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.IO;

namespace Segmentation_and_other
{
    class MainPresenter
    {
        private readonly IForm1 _form1;
        //private readonly IStorageDevices _storageDevices;
        private readonly IMessageService _messageService;
        private readonly ILogicalDisksProcessing _logicalDiskProcessing;

        List<LogicalDisks> ListLD = new List<LogicalDisks>();
        List<string> NameLD = new List<string>();
        List<string> TypeLD = new List<string>();

        List<Int64> SizeObj = new List<Int64>();
        List<int> TypeObj = new List<int>();
        List<string> NameObj = new List<string>(); 
        List<string> NameFiles = new List<string>();

        Int32 Encod=1200; //по умолчанию Unicode

        Int32 IndexUserPathInList = 0;
        List<Tuple<string, Int32, Int32, string>> PathComponentSys_FistKlasterDir_NumLD_PathToView = new List<Tuple<string, Int32, Int32, string>>(); //Путь пройденный пользователем. для ntfs номер первого кластера дир пишу =0

        int CountOfKnownLD = 0;
        public MainPresenter (IForm1 form1, List<LogicalDisks> logicalDisks, IMessageService messageService, ILogicalDisksProcessing logicalDisksProcessing)
        {
           
            _form1 = form1;
            _messageService = messageService;
            _logicalDiskProcessing = logicalDisksProcessing;

            #region Инфа при старте приложухи

            int NumOfBootDrive =-1;
            string BootDrive = Environment.GetEnvironmentVariable("SystemDrive") + "\\";
            string TypeFSofBootDrive = "";

            for (int i = 0; i < logicalDisks.Count; i++)
            {
                if(logicalDisks[i].SG_NameLD != "Nop!Oopsee! " && (logicalDisks[i].SG_TypeFileSyst == "FAT32" || logicalDisks[i].SG_TypeFileSyst == "NTFS"))
                {
                    NameLD.Add(logicalDisks[i].SG_NameLD);
                    TypeLD.Add(logicalDisks[i].SG_TypeFileSyst);
                    ListLD.Add(logicalDisks[i]);

                    if (logicalDisks[i].SG_NameLD == BootDrive)
                    {
                        NumOfBootDrive = NameLD.Count - 1;
                        TypeFSofBootDrive = logicalDisks[i].SG_TypeFileSyst;
                    }
                }
            }//заполнение списка имен логических дисков снасала fat32 и ntfs с именами

            CountOfKnownLD = ListLD.Count;

            for (int i = 0; i < logicalDisks.Count; i++)
            {
                if (logicalDisks[i].SG_NameLD != "Nop!Oopsee! " && logicalDisks[i].SG_TypeFileSyst != "FAT32" && logicalDisks[i].SG_TypeFileSyst != "NTFS")
                {
                    NameLD.Add(logicalDisks[i].SG_NameLD);
                    TypeLD.Add(logicalDisks[i].SG_TypeFileSyst);
                    ListLD.Add(logicalDisks[i]);

                    if (logicalDisks[i].SG_NameLD == BootDrive)
                    {
                        NumOfBootDrive = NameLD.Count - 1;
                        TypeFSofBootDrive = logicalDisks[i].SG_TypeFileSyst;
                    }
                }
            }//заполнение списка имен логических дисков с именами, но не fat32 и ntfs

            int countULD = 0;
            for (int i = 0; i < logicalDisks.Count; i++)
            {
                if (logicalDisks[i].SG_NameLD == "Nop!Oopsee! ")
                {
                    NameLD.Add("UnknownLD - " + countULD);
                    TypeLD.Add(logicalDisks[i].SG_TypeFileSyst);
                    ListLD.Add(logicalDisks[i]);

                    if (logicalDisks[i].SG_NameLD == BootDrive)
                    {
                        NumOfBootDrive = NameLD.Count - 1;
                        TypeFSofBootDrive = logicalDisks[i].SG_TypeFileSyst;
                    }
                    countULD++;
                }
            }//заполнение списка имен логических дисков без имен

            _form1.LoadDataToListViewLogicalDisks(NameLD, NumOfBootDrive);     //вывод в форму списка лд

            if (TypeFSofBootDrive == "NTFS")
            {
                DirectoryInfo dInfo = new DirectoryInfo(BootDrive);
                FileInfo[] FInfo = dInfo.GetFiles();
                DirectoryInfo[] DInfo = dInfo.GetDirectories();

                SizeObj.Clear();
                TypeObj.Clear();
                NameObj.Clear();

                for (int i = 0; i < DInfo.Length; i++)
                {
                    SizeObj.Add(0);//заполняю для вывода размер папок = 0
                    TypeObj.Add(0);//заполняю тип файла Папка=0 
                    NameObj.Add(DInfo[i].Name);
                }

                for(int i = 0; i < FInfo.Length; i++) 
                {
                    SizeObj.Add(FInfo[i].Length);//заполняю для вывода размер файлов
                    TypeObj.Add(1);//заполняю тип файла Файл=1
                    NameObj.Add(FInfo[i].Name);
                }

                //NameObj.AddRange(NameFiles);

                PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(ListLD[NumOfBootDrive].SG_NameLD, 0, NumOfBootDrive, ListLD[NumOfBootDrive].SG_NameLD));
                IndexUserPathInList = 0;
                _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                _form1.SG_txTotalPath = dInfo.FullName;

            }

            else if (TypeFSofBootDrive == "FAT32")
            {
                Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res;

                Res = _logicalDiskProcessing.FindAllObjInFat32Directory(ListLD[NumOfBootDrive], ListLD[NumOfBootDrive].SG_FirstKlasterKornevohoKataloga);

                SizeObj.Clear();
                TypeObj.Clear();
                NameObj.Clear();

                for (int j = 0; j < Res.Item4.Count; j++)
                {
                    SizeObj.Add(0);//заполняю для вывода размер папок = 0
                    TypeObj.Add(0);//заполняю тип файла Папка=0 
                    NameObj.Add(Res.Item4[j]);
                }

                for (int j = 0; j < Res.Item4.Count; j++)
                {
                    SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                    TypeObj.Add(1);//заполняю тип файла Файл=1
                    NameObj.Add(Res.Item1[j]);
                }

                PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(ListLD[NumOfBootDrive].SG_NameLD, ListLD[NumOfBootDrive].SG_FirstKlasterKornevohoKataloga, NumOfBootDrive, ListLD[NumOfBootDrive].SG_NameLD));
                IndexUserPathInList = 0;
                _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                _form1.SG_txTotalPath = ListLD[NumOfBootDrive].SG_NameLD;
            }

            //заполнение comboBoxEncod доступными кодировками
            List<string> AvailableEncodings = new List<string>();
            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                Encoding e = ei.GetEncoding();
                AvailableEncodings.Add(ei.CodePage + "   -   " + ei.Name);               
            }

            #endregion

            #region Ловлю события

            _form1.ComboBoxEncodingAddListItems(AvailableEncodings);
            _form1.EnterSlovoIDZClick += _form1_EnterSlovoIDZClick;
            _form1.EnterPuteClick += _form1_EnterPuteClick;
            _form1.BackwardFileManagerClick += _form1_BackwardFileManagerClick;
            _form1.ForwardFileManagerClick += _form1_ForwardFileManagerClick;
            _form1.LogicalDisksMouseDoubleClick += _form1_LogicalDisksMouseDoubleClick;
            _form1.FileManagerMouseDoubleClick += _form1_FileManagerMouseDoubleClick;
            _form1.comboBoxEncodingSelectedItem += _form1_comboBoxEncodingSelectedItem;
            _logicalDiskProcessing.TickToProgressBar += _logicalDiskProcessing_TickToProgressBar;
            _logicalDiskProcessing.ResetProgressBar += _logicalDiskProcessing_ResetProgressBar;
            _form1.LogicalDisksMouseRightClick += _form1_LogicalDisksMouseRightClick;

            #endregion

        }

        #region Обработка событий
        private void _form1_LogicalDisksMouseRightClick(object sender, EventArgs e)
        {
           int SelectedNum = _form1.SG_NumRightClickLD;

            string LDinfo = "Имя логического диска: " + ListLD[SelectedNum].SG_NameLD + System.Environment.NewLine;
            LDinfo = LDinfo + "Имя физического устройства: " + ListLD[SelectedNum].SG_NameStorageDevice + System.Environment.NewLine;
            LDinfo = LDinfo + "Тип файловой системы: " + ListLD[SelectedNum].SG_TypeFileSyst + System.Environment.NewLine;

            //string LDinfo = "Имя физического устройства: " + ListLD[SelectedNum].SG_NameStorageDevice + System.Environment.NewLine;
            // LDinfo = LDinfo + "ID устройства в Windows: " + ListLD[SelectedNum].SG_DiskID + System.Environment.NewLine;
            // LDinfo = LDinfo + "Handle физического устройства: " + ListLD[SelectedNum].SG_Handle + System.Environment.NewLine;
            // LDinfo = LDinfo + "Первичная MBR целая: " + ListLD[SelectedNum].SG_IsGood + System.Environment.NewLine;
            // LDinfo = LDinfo + "Тип сегментации физического устройства: " + ListLD[SelectedNum].SG_TypeOfSegmentation + System.Environment.NewLine;

            // LDinfo = LDinfo + "Имя логического диска: " + ListLD[SelectedNum].SG_NameLD+ System.Environment.NewLine; 
            // LDinfo = LDinfo + "Тип файловой системы: " + ListLD[SelectedNum].SG_TypeFileSyst + System.Environment.NewLine;         
            // LDinfo = LDinfo + "Размер кластера в байтах: " + ListLD[SelectedNum].SG_KlasterSize + System.Environment.NewLine;

            _messageService.ShowMessage(LDinfo);
        }//+
        private void _logicalDiskProcessing_ResetProgressBar(object sender, EventArgs e)
        {
            _form1.SG_progressBarOperationValue =  0;
        }//+

        private void _logicalDiskProcessing_TickToProgressBar(object sender, EventArgs e)
        {
            int PlusTick = _logicalDiskProcessing.SG_NumPlusToProgressBar;
            _form1.SG_progressBarOperationValue = _form1.SG_progressBarOperationValue + PlusTick;
        }//+

        private void _form1_comboBoxEncodingSelectedItem(object sender, EventArgs e)
        {
           string EncodingStr = _form1.SG_ComboBoxEncodingSelectedItem;
            EncodingStr = EncodingStr.Remove(EncodingStr.IndexOf(" "), EncodingStr.Length - EncodingStr.IndexOf(" "));
            Encod = Convert.ToInt32(EncodingStr);
        }//+

        private void _form1_FileManagerMouseDoubleClick(object sender, EventArgs e)
        {
            string SelectItem = _form1.SG_NameSelectedObj;
            int NumSelectItem = _form1.SG_NumSelectedObj;

            if (TypeObj[NumSelectItem] == 0)
            {
                if(PathComponentSys_FistKlasterDir_NumLD_PathToView.Count-1 > IndexUserPathInList)
                {
                    PathComponentSys_FistKlasterDir_NumLD_PathToView.RemoveRange(IndexUserPathInList + 1, PathComponentSys_FistKlasterDir_NumLD_PathToView.Count - (IndexUserPathInList+1));
                }
                
                if (ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3].SG_TypeFileSyst == "FAT32")
                {
                    Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, Int32> Res;

                    Res = _logicalDiskProcessing.FindAllObjInNextFat32Directory(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3], PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item2, SelectItem);

                    SizeObj.Clear();
                    TypeObj.Clear();
                    NameObj.Clear();

                    for (int j = 0; j < Res.Item4.Count; j++)
                    {
                        if (Res.Item4[j] != "." && Res.Item4[j] != "...")
                        {
                            SizeObj.Add(0);//заполняю для вывода размер папок = 0
                            TypeObj.Add(0);//заполняю тип файла Папка=0 
                            NameObj.Add(Res.Item4[j]);
                        }
                    }

                    for (int j = 0; j < Res.Item1.Count; j++)
                    {
                        SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                        TypeObj.Add(1);//заполняю тип файла Файл=1
                        NameObj.Add(Res.Item1[j]);
                    }

                    _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);

                    //формирование пути для отображения
                    string PathViewStr = "";
                    for (int i = PathComponentSys_FistKlasterDir_NumLD_PathToView.Count-1; i > 0; i--)
                    {
                        if(PathComponentSys_FistKlasterDir_NumLD_PathToView[i].Item1.Contains(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[i].Item3].SG_NameLD))
                        {
                            for (int h = i; h < PathComponentSys_FistKlasterDir_NumLD_PathToView.Count; h++)
                            {
                                if (PathComponentSys_FistKlasterDir_NumLD_PathToView[h].Item1 == ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[i].Item3].SG_NameLD)
                                {
                                    PathViewStr = PathViewStr + PathComponentSys_FistKlasterDir_NumLD_PathToView[h].Item1;
                                }
                                else
                                {
                                    PathViewStr = PathViewStr + PathComponentSys_FistKlasterDir_NumLD_PathToView[h].Item1 + "\\";
                                }
                            }
                            PathViewStr = PathViewStr + SelectItem;
                            break;
                        }
                    }

                    PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(SelectItem, Res.Item7, PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3, PathViewStr));
                    IndexUserPathInList++;

                    //_form1.SG_txTotalPath = Convert.ToString(_form1.SG_txTotalPath + "\\" + SelectItem);//переписать!
                    _form1.SG_txTotalPath = Convert.ToString(PathViewStr);
                }
                else
                {
                    string PathViewStr = "";
                    for (int i = PathComponentSys_FistKlasterDir_NumLD_PathToView.Count - 1; i > -1; i--)
                    {
                        if (PathComponentSys_FistKlasterDir_NumLD_PathToView[i].Item1.Contains(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[i].Item3].SG_NameLD))
                        {
                            for (int h = i; h < PathComponentSys_FistKlasterDir_NumLD_PathToView.Count; h++)
                            {
                                if (PathComponentSys_FistKlasterDir_NumLD_PathToView[h].Item1 == ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[i].Item3].SG_NameLD)
                                {
                                    PathViewStr = PathViewStr + PathComponentSys_FistKlasterDir_NumLD_PathToView[h].Item1;
                                }
                                else
                                {
                                    PathViewStr = PathViewStr + PathComponentSys_FistKlasterDir_NumLD_PathToView[h].Item1 + "\\";
                                }
                            }
                            PathViewStr = PathViewStr + SelectItem;
                            break;
                        }
                    }


                    DirectoryInfo dInfo = new DirectoryInfo(PathViewStr);
                    FileInfo[] FInfo=null;
                    try
                    {
                        FInfo = dInfo.GetFiles();
                    }
                    catch
                    {
                        _messageService.ShowError("Отказано в доступе!");
                        //_messageService.ShowMessage(PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4);
                        //_messageService.ShowMessage(PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item1);
                        return;
                    }
                    DirectoryInfo[] DInfo = dInfo.GetDirectories();

                    SizeObj.Clear();
                    TypeObj.Clear();
                    NameObj.Clear();
                    //NameFiles.Clear();

                    for (int j = 0; j < DInfo.Length; j++)
                    {
                        SizeObj.Add(0);//заполняю для вывода размер папок = 0
                        TypeObj.Add(0);//заполняю тип файла Папка=0 
                        NameObj.Add(DInfo[j].Name);
                    }

                    for (int j = 0; j < FInfo.Length; j++)
                    {
                        SizeObj.Add(FInfo[j].Length);//заполняю для вывода размер файлов
                        TypeObj.Add(1);//заполняю тип файла Файл=1
                        NameObj.Add(FInfo[j].Name);
                    }

                    // NameObj.AddRange(NameFiles);

                    _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                    _form1.SG_txTotalPath = dInfo.FullName;

                    PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(SelectItem, 0, PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3, dInfo.FullName));
                    IndexUserPathInList++;
                  
                }
            }

        }//+

        private void _form1_LogicalDisksMouseDoubleClick(object sender, EventArgs e)
        {
            string NameSelectedLD = _form1.SG_NameSelectedLD;
            int NumSelectedLD = _form1.SG_NumSelectedLD;

            if (NumSelectedLD < CountOfKnownLD)
            {
                _form1.LoadDataToListViewLogicalDisks(NameLD, NumSelectedLD);
                _form1.SG_txTotalPath = Convert.ToString(NameSelectedLD);

                for (int i = 0; i < ListLD.Count; i++)
                {
                    if (ListLD[i].SG_NameLD == NameSelectedLD)
                    {
                        if (ListLD[i].SG_TypeFileSyst == "FAT32")
                        {
                            Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res;

                            Res = _logicalDiskProcessing.FindAllObjInFat32Directory(ListLD[i], ListLD[i].SG_FirstKlasterKornevohoKataloga);

                            SizeObj.Clear();
                            TypeObj.Clear();
                            NameObj.Clear();

                            for (int j = 0; j < Res.Item4.Count; j++)
                            {
                                SizeObj.Add(0);//заполняю для вывода размер папок = 0
                                TypeObj.Add(0);//заполняю тип файла Папка=0 
                                NameObj.Add(Res.Item4[j]);
                            }

                            for (int j = 0; j < Res.Item1.Count; j++)
                            {
                                SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                                TypeObj.Add(1);//заполняю тип файла Файл=1
                                NameObj.Add(Res.Item1[j]);
                            }

                            _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);

                            PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(ListLD[i].SG_NameLD, ListLD[i].SG_FirstKlasterKornevohoKataloga, i, ListLD[i].SG_NameLD));
                            IndexUserPathInList++;
                            break;
                        }

                        else if (ListLD[i].SG_TypeFileSyst == "NTFS")
                        {
                            DirectoryInfo dInfo = new DirectoryInfo(NameSelectedLD);
                            FileInfo[] FInfo = dInfo.GetFiles();
                            DirectoryInfo[] DInfo = dInfo.GetDirectories();

                            SizeObj.Clear();
                            TypeObj.Clear();
                            NameObj.Clear();
                            //NameFiles.Clear();

                            for (int j = 0; j < DInfo.Length; j++)
                            {
                                SizeObj.Add(0);//заполняю для вывода размер папок = 0
                                TypeObj.Add(0);//заполняю тип файла Папка=0 
                                NameObj.Add(DInfo[j].Name);
                            }

                            for (int j = 0; j < FInfo.Length; j++)
                            {
                                SizeObj.Add(FInfo[j].Length);//заполняю для вывода размер файлов
                                TypeObj.Add(1);//заполняю тип файла Файл=1
                                NameObj.Add(FInfo[j].Name);
                            }

                            // NameObj.AddRange(NameFiles);

                            _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                            _form1.SG_txTotalPath = dInfo.FullName;

                            PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(ListLD[i].SG_NameLD, 0, i, dInfo.FullName));
                            IndexUserPathInList++;
                            break;
                        }
                    }
                }
            }
            else
            {
                _messageService.ShowError("Выбранный логический диск не поддерживается!");
            }

        }//+

        private void _form1_ForwardFileManagerClick(object sender, EventArgs e)
        {
            if (IndexUserPathInList < PathComponentSys_FistKlasterDir_NumLD_PathToView.Count - 1)
            {
                IndexUserPathInList++;
                if (ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3].SG_NameLD == PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item1)
                {
                    _form1.LoadDataToListViewLogicalDisks(NameLD, PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3);
                }

                if (ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3].SG_TypeFileSyst == "FAT32")
                {
                    Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res;

                    Res = _logicalDiskProcessing.FindAllObjInFat32Directory(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3], PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item2);

                    SizeObj.Clear();
                    TypeObj.Clear();
                    NameObj.Clear();

                    for (int j = 0; j < Res.Item4.Count; j++)
                    {
                        if (Res.Item4[j] != "." && Res.Item4[j] != "...")
                        {
                            SizeObj.Add(0);//заполняю для вывода размер папок = 0
                            TypeObj.Add(0);//заполняю тип файла Папка=0 
                            NameObj.Add(Res.Item4[j]);
                        }
                    }

                    for (int j = 0; j < Res.Item1.Count; j++)
                    {
                        SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                        TypeObj.Add(1);//заполняю тип файла Файл=1
                        NameObj.Add(Res.Item1[j]);
                    }

                    _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);


                    //переписать!
                    // _form1.SG_txTotalPath = _form1.SG_txTotalPath + "\\" + PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item1;
                    _form1.SG_txTotalPath = PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4;
                }
                else
                {

                    DirectoryInfo dInfo = new DirectoryInfo(PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4);
                    FileInfo[] FInfo = dInfo.GetFiles();
                    DirectoryInfo[] DInfo = dInfo.GetDirectories();

                    SizeObj.Clear();
                    TypeObj.Clear();
                    NameObj.Clear();
                    //NameFiles.Clear();

                    for (int j = 0; j < DInfo.Length; j++)
                    {
                        SizeObj.Add(0);//заполняю для вывода размер папок = 0
                        TypeObj.Add(0);//заполняю тип файла Папка=0 
                        NameObj.Add(DInfo[j].Name);
                    }

                    for (int j = 0; j < FInfo.Length; j++)
                    {
                        SizeObj.Add(FInfo[j].Length);//заполняю для вывода размер файлов
                        TypeObj.Add(1);//заполняю тип файла Файл=1
                        NameObj.Add(FInfo[j].Name);
                    }

                    // NameObj.AddRange(NameFiles);

                    _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                    _form1.SG_txTotalPath = dInfo.FullName;
                }
            }
        }//+

        private void _form1_BackwardFileManagerClick(object sender, EventArgs e)
        {
            if (IndexUserPathInList > 0 && PathComponentSys_FistKlasterDir_NumLD_PathToView.Count > 0)
            {
                IndexUserPathInList--;

                if (PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4.Contains(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3].SG_NameLD))
                {
                    _form1.LoadDataToListViewLogicalDisks(NameLD, PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3);
                }

                if (ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3].SG_TypeFileSyst == "FAT32")
                {
                    Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>> Res;

                    Res = _logicalDiskProcessing.FindAllObjInFat32Directory(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3], PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item2);

                    SizeObj.Clear();
                    TypeObj.Clear();
                    NameObj.Clear();

                    for (int j = 0; j < Res.Item4.Count; j++)
                    {
                        if (Res.Item4[j] != "." && Res.Item4[j] != "...")
                        {
                            SizeObj.Add(0);//заполняю для вывода размер папок = 0
                            TypeObj.Add(0);//заполняю тип файла Папка=0 
                            NameObj.Add(Res.Item4[j]);
                        }
                    }

                    for (int j = 0; j < Res.Item1.Count; j++)
                    {
                        SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                        TypeObj.Add(1);//заполняю тип файла Файл=1
                        NameObj.Add(Res.Item1[j]);
                    }

                    _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                    
                    //int NumDel = _form1.SG_txTotalPath.LastIndexOf("\\");//переписать!
                    //string NewPath = _form1.SG_txTotalPath.Remove(NumDel);//переписать!


                    _form1.SG_txTotalPath = PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4;
                }
                else
                {


                    DirectoryInfo dInfo = new DirectoryInfo(PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4);
                    FileInfo[] FInfo = dInfo.GetFiles();
                    DirectoryInfo[] DInfo = dInfo.GetDirectories();

                    SizeObj.Clear();
                    TypeObj.Clear();
                    NameObj.Clear();
                    //NameFiles.Clear();

                    for (int j = 0; j < DInfo.Length; j++)
                    {
                        SizeObj.Add(0);//заполняю для вывода размер папок = 0
                        TypeObj.Add(0);//заполняю тип файла Папка=0 
                        NameObj.Add(DInfo[j].Name);
                    }

                    for (int j = 0; j < FInfo.Length; j++)
                    {
                        SizeObj.Add(FInfo[j].Length);//заполняю для вывода размер файлов
                        TypeObj.Add(1);//заполняю тип файла Файл=1
                        NameObj.Add(FInfo[j].Name);
                    }

                    // NameObj.AddRange(NameFiles);

                    _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                    _form1.SG_txTotalPath = dInfo.FullName;

                }
            }
        }//+

        private void _form1_EnterPuteClick(object sender, EventArgs e)
        {
            
            String Path = _form1.SG_txTotalPath;
            string Bukva = "";

            List<Tuple<string, string>> Namespace;
            Namespace = _logicalDiskProcessing.GetNamespace(Path);

            if(Namespace[0].Item2 == "Good")
            {
                Bukva = Namespace[0].Item1;
            }


            for (int i = 0; i < ListLD.Count; i++)
            {
                if(Bukva == ListLD[i].SG_NameLD)
                {
                    if(ListLD[i].SG_TypeFileSyst == "NTFS")
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(Path);
                        if (!dInfo.Exists)
                        {
                            _messageService.ShowError("Указанный путь несуществует!");
                        }
                        FileInfo[] FInfo = null;
                        try
                        {
                            FInfo = dInfo.GetFiles();
                        }
                        catch
                        {
                            _messageService.ShowError("Отказано в доступе!");
                            return;
                        }
                        DirectoryInfo[] DInfo = dInfo.GetDirectories();

                        SizeObj.Clear();
                        TypeObj.Clear();
                        NameObj.Clear();
                        //NameFiles.Clear();

                        for (int j = 0; j < DInfo.Length; j++)
                        {
                            SizeObj.Add(0);//заполняю для вывода размер папок = 0
                            TypeObj.Add(0);//заполняю тип файла Папка=0 
                            NameObj.Add(DInfo[j].Name);
                        }

                        for (int j = 0; j < FInfo.Length; j++)
                        {
                            SizeObj.Add(FInfo[j].Length);//заполняю для вывода размер файлов
                            TypeObj.Add(1);//заполняю тип файла Файл=1
                            NameObj.Add(FInfo[j].Name);
                        }

                        // NameObj.AddRange(NameFiles);

                        _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);
                        _form1.SG_txTotalPath = dInfo.FullName;

                        PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(Path, 0, i, dInfo.FullName));
                        IndexUserPathInList++;
                    }
                    else if(ListLD[i].SG_TypeFileSyst == "FAT32")
                    {
                        Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, Int32> Res;

                        Res = _logicalDiskProcessing.FindAllObjInFat32DirectoryPath(ListLD[i], Path);

                        SizeObj.Clear();
                        TypeObj.Clear();
                        NameObj.Clear();

                        for (int j = 0; j < Res.Item4.Count; j++)
                        {
                            if (Res.Item4[j] != "." && Res.Item4[j] != "...")
                            {
                                SizeObj.Add(0);//заполняю для вывода размер папок = 0
                                TypeObj.Add(0);//заполняю тип файла Папка=0 
                                NameObj.Add(Res.Item4[j]);
                            }
                        }

                        for (int j = 0; j < Res.Item1.Count; j++)
                        {
                            SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                            TypeObj.Add(1);//заполняю тип файла Файл=1
                            NameObj.Add(Res.Item1[j]);
                        }

                        _form1.LoadDataToListViewLogicalDisks(NameLD, i);//подсвечиваю диск на который осуществлен переход
                        _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, null);

                        PathComponentSys_FistKlasterDir_NumLD_PathToView.Add(Tuple.Create(/*Namespace[Namespace.Count-1].Item1*/Path, Res.Item7, i, Path));
                        IndexUserPathInList++;
                        break;
                    }
                }
            }
        }//+

        private void _form1_EnterSlovoIDZClick(object sender, EventArgs e)
        {

            _form1.SG_progressBarOperationValue = 0;
            string Slovo = _form1.SG_txSlovo;

            if(Slovo=="")
            {
                _messageService.ShowMessage("Вы ищете пустую строку!");
            }
           
            if (ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3].SG_TypeFileSyst == "FAT32")
            {
                Tuple<List<string>, List<Int32>, List<UInt32>, List<string>, List<Int32>, List<UInt32>, List<Int32>> Res;
                Res = _logicalDiskProcessing.FindAllFilesWithSlovoInCurrentFat32Directory(ListLD[PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item3], PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item2, Slovo, Encod);

                SizeObj.Clear();
                TypeObj.Clear();
                NameObj.Clear();

                for (int j = 0; j < Res.Item4.Count; j++)
                {
                    if (Res.Item4[j] != "." && Res.Item4[j] != "...")
                    {
                        SizeObj.Add(0);//заполняю для вывода размер папок = 0
                        TypeObj.Add(0);//заполняю тип файла Папка=0 
                        NameObj.Add(Res.Item4[j]);
                    }
                }

                for (int j = 0; j < Res.Item1.Count; j++)
                {
                    SizeObj.Add(Res.Item3[j]);//заполняю для вывода размер файлов
                    TypeObj.Add(1);//заполняю тип файла Файл=1
                    NameObj.Add(Res.Item1[j]);
                }

                _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, Res.Item7);

                int nums = 0;
                for(int gg = 0; gg < Res.Item7.Count; gg++)
                {
                    if(Res.Item7[gg]==-1)
                    {
                        nums++;
                    }
                }
                if(nums == Res.Item7.Count)
                {
                    _messageService.ShowMessage("Строки в файлах не найденно!");
                }
                
            }
            else
            {
                List<Int32> HighlightItems = new List<int>();
                HighlightItems = _logicalDiskProcessing.FindAllFilesWithSlovoInCurrentNTFSDirectory(TypeObj, NameObj, Slovo, PathComponentSys_FistKlasterDir_NumLD_PathToView[IndexUserPathInList].Item4);

                _form1.LoadDataToListViewFileManager(NameObj, TypeObj, SizeObj, HighlightItems);

                int nums = 0;
                for (int gg = 0; gg < HighlightItems.Count; gg++)
                {
                    if (HighlightItems[gg] == -1)
                    {
                        nums++;
                    }
                }
                if (nums == HighlightItems.Count)
                {
                    _messageService.ShowMessage("Строки в файлах не найденно!");
                }
            }

        }//+

        #endregion
    }
}
