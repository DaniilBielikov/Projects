using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;

namespace Segmentation_and_other
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
 
            Form1 form = new Form1();
            StorageDevices storageDevices = new StorageDevices();
            List <LogicalDisks> logicalDisks = new List<LogicalDisks>();
            MessageService service = new MessageService();
            LogicalDisksProcessing LDprocessing = new LogicalDisksProcessing();

            int j = 0, num = 0;
            StorageDevices AA1 = new StorageDevices();
            LogicalDisks LD = new LogicalDisks();

            List<string> DiskID = new List<string>();
            List<string> DiskName = new List<string>();

            List<Tuple<string, string, UInt64>> List_NameLD_TypeFileSyst_AdrNuchLD = new List<Tuple<string, string, UInt64>>();

            Tuple<UInt32, UInt64, UInt64, UInt32, Int32, UInt64, Int32> LD_Info;
            List<LogicalDisks> LogicalDisksList = new List<LogicalDisks>();

            DiskID = AA1.WMI_Carry("Win32_DiskDrive", "DeviceID");
            DiskName = AA1.WMI_Carry("Win32_DiskDrive", "MODEL");

            //проход по поиску физических запоминающих устройств
            for (int i = 0; i < DiskID.Count; i++)
            {
                j = 0;

                AA1.SG_NameStorageDevice = DiskName[i];
                AA1.SG_DiskID = DiskID[i];
                AA1.SG_Handle = AA1.GetHandle(AA1.SG_DiskID);
                AA1.SG_IsGood = AA1.GetIsGood(AA1.SG_Handle);

                if (AA1.SG_IsGood == true)
                {
                    AA1.SG_TypeOfSegmentation = AA1.GetTypeOfSegmentation(AA1.SG_Handle);

                    List_NameLD_TypeFileSyst_AdrNuchLD.Clear();
                    //заполнение объектов логич. дисков полное

                    do
                    {
                        LogicalDisksList.Add(new LogicalDisks());
                        LogicalDisks.countLD++;

                        LogicalDisksList[num].SG_NameStorageDevice = AA1.SG_NameStorageDevice;
                        LogicalDisksList[num].SG_DiskID = AA1.SG_DiskID;
                        LogicalDisksList[num].SG_Handle = AA1.SG_Handle;
                        LogicalDisksList[num].SG_IsGood = AA1.SG_IsGood;
                        LogicalDisksList[num].SG_TypeOfSegmentation = AA1.SG_TypeOfSegmentation;

                        if (j == 0)
                        {
                            List_NameLD_TypeFileSyst_AdrNuchLD = LD.Get_List_NameLD_TypeFileSyst_AdrNuchLD(LogicalDisksList[num].SG_Handle, LogicalDisksList[num].SG_TypeOfSegmentation, LogicalDisksList[num].SG_IsGood);
                        }
                        if (List_NameLD_TypeFileSyst_AdrNuchLD.Count > 0)
                        {
                            //заполнение obj.LD о инфе о самом логич. диске устройства
                            LogicalDisksList[num].SG_NameLD = List_NameLD_TypeFileSyst_AdrNuchLD[j].Item1;
                            LogicalDisksList[num].SG_TypeFileSyst = List_NameLD_TypeFileSyst_AdrNuchLD[j].Item2;
                            LogicalDisksList[num].SG_AdrNuch = List_NameLD_TypeFileSyst_AdrNuchLD[j].Item3;

                            LD_Info = LogicalDisksList[num].Get_LD_Info(LogicalDisksList[num]);
                            LogicalDisksList[num].SG_KlasterSize = LD_Info.Item1;
                            LogicalDisksList[num].SG_AdrRoot = LD_Info.Item2;
                            LogicalDisksList[num].SG_AdrFat = LD_Info.Item3;
                            LogicalDisksList[num].SG_SizeFat = LD_Info.Item4;
                            LogicalDisksList[num].SG_ActiveTableFat = LD_Info.Item5;
                            LogicalDisksList[num].SG_SmeshenieActiveTableFat = LD_Info.Item6;
                            LogicalDisksList[num].SG_FirstKlasterKornevohoKataloga = LD_Info.Item7;
                        }

                        j++;
                        num++;
                    }
                    while (j < List_NameLD_TypeFileSyst_AdrNuchLD.Count);
                }
                else
                {
                    AA1.CloseHandleDisk(AA1.SG_Handle);
                   
                }
            }

            MainPresenter presenter = new MainPresenter(form, LogicalDisksList, service, LDprocessing);
            
            Application.Run(form);
        }
    }
}
