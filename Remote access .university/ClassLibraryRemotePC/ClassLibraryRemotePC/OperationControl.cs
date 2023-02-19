using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryRemotePC
{
    internal class OperationControl
    {
        static object locker = new object();
        static Semaphore OpSem = new Semaphore(1, 1);
        List<OperationFlags> operationFlags = new List<OperationFlags>();
        List<OperationCod> operationCods = new List<OperationCod>();

        List<OperationCod> operationCods_AddingBuff = new List<OperationCod>();
        List<OperationCod> operationCods_RemovingBuff = new List<OperationCod>();

        internal event EventHandler RunOperationCode;

        internal int ControlThreadSleepingTime
        { set; get; } = 75;    

        internal void AddOperationCod(OperationCod operationCod)
        {
            operationCods_AddingBuff.Add(operationCod);
        }
        private void AnalysisOperationCode(OperationCod operationCod)
        {
            bool FlgObjFound = false;
            bool FlgLinkedPCObjFound = false;
            bool ForbiddenTypeFound1 = false;
            bool ForbiddenTypeFound2 = false;

            if (operationCod.OperationStatus == 0)
            {
                if (operationFlags.Count != 0)
                {
                    for (int i = 0; i < operationFlags.Count; i++)
                    {
                        if (operationFlags[i].NumberPC == operationCod.NumberPC)
                        {
                            FlgObjFound = true;
                            for (int j = 0; j < operationFlags[i].ForbiddenTypes.Count; j++)
                            {
                                for (int k = 0; k < operationCod.OperationTypes.Count; k++)
                                {
                                    if (operationFlags[i].ForbiddenTypes[j] == operationCod.OperationTypes[k])
                                    {
                                        ForbiddenTypeFound1 = true;
                                        break;
                                    }//если найденно соответствие в запрещенных типах
                                }
                                if (ForbiddenTypeFound1 == true)
                                    break;
                            }//анализ флагов
                            if (ForbiddenTypeFound1 == false)
                            {
                                if (operationCod.IsChain == true)
                                {
                                    if (operationCod.NumberLinkedPC != operationCod.NumberPC)
                                    {
                                        for (int p = 0; p < operationFlags.Count; p++)
                                        {
                                            if (operationFlags[p].NumberPC == operationCod.NumberLinkedPC)
                                            {
                                                FlgLinkedPCObjFound = true;
                                                for (int j = 0; j < operationFlags[p].ForbiddenTypes.Count; j++)
                                                {
                                                    for (int k = 0; k < operationCod.OperationTypes.Count; k++)
                                                    {
                                                        if (operationFlags[p].ForbiddenTypes[j] == operationCod.OperationTypes[k])
                                                        {
                                                            ForbiddenTypeFound2 = true;
                                                            break;
                                                        }//если найденно соответствие в запрещенных типах
                                                    }
                                                    if (ForbiddenTypeFound2 == true)
                                                        break;
                                                }
                                                if (ForbiddenTypeFound2 == false)
                                                {
                                                    operationFlags[i].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                                                    operationFlags[p].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                                                    operationCod.OperationStatus = 1;
                                                    if (RunOperationCode != null) RunOperationCode(operationCod, EventArgs.Empty);
                                                }//заполнить оба объекта флагов и запуск КОП
                                            }//если объект флагов для указаного номера ПК найден
                                        }//поиск объекта флага для связанного ПК
                                        if (FlgLinkedPCObjFound == false)
                                        {
                                            operationFlags[i].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                                            operationFlags.Add(new OperationFlags(operationCod.NumberLinkedPC));
                                            operationFlags[operationFlags.Count - 1].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                                            operationCod.OperationStatus = 1;
                                            if (RunOperationCode != null) RunOperationCode(operationCod, EventArgs.Empty);
                                        }//если объекта флага для связанного ПК не найдено. Создать объект флага для связанного ПК, заполнить оба объекта флагов и запуск КОП
                                    }//если цепочка относится к одному ПК
                                    else
                                    {
                                        operationFlags[i].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                                        operationCod.OperationStatus = 1;
                                        if (RunOperationCode != null) RunOperationCode(operationCod, EventArgs.Empty);
                                    }//если КОП относится к цепочке одного ПК
                                }//Если КОП относится к цепочке КОП
                                else
                                {
                                    operationFlags[i].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                                    operationCod.OperationStatus = 1;
                                    if (RunOperationCode != null) RunOperationCode(operationCod, EventArgs.Empty);
                                }//Если КОП не относится к цепочке КОП
                            }//коп не содержит запрещенных на данный момент типов операций
                            break;
                        }//если объект флагов для указаного номера ПК найден
                    }//поиск объекта с флагами принадлежащего компьютеру указанному в коп
                }//если есть объекты флагов
                if (FlgObjFound == false)
                {
                    operationFlags.Add(new OperationFlags(operationCod.NumberPC));
                    operationFlags[operationFlags.Count - 1].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                    if (operationCod.NumberPC != operationCod.NumberLinkedPC && operationCod.IsChain == true)
                    {
                        operationFlags.Add(new OperationFlags(operationCod.NumberLinkedPC));
                        operationFlags[operationFlags.Count - 1].ForbiddenTypes = new List<ushort>(operationCod.OperationTypes);
                    }//если это цепочка КОП и она принадлежит не одному ПК, а связке из 2х ПК
                    operationCod.OperationStatus = 1;
                    if (RunOperationCode != null) RunOperationCode(operationCod, EventArgs.Empty);
                }//объект флагов для данного номер пк не найден
            }//статус коп "не установлен на выполнение"
        }
        internal void OperationControlModul()
        {
            while (true)
            {
                OpSem.WaitOne();

                foreach (OperationCod operationCod in operationCods)
                {
                    AnalysisOperationCode(operationCod);
                }
                if (operationCods_AddingBuff.Count() > 0)
                {
                    for(int i = 0; i < operationCods_AddingBuff.Count(); i++)
                    {
                        operationCods.Add(operationCods_AddingBuff[i]);
                    }
                    operationCods_AddingBuff.Clear();
                }
                if (operationCods_RemovingBuff.Count() > 0)
                {
                    //////////////----
                    //bool Status = false;
                    //for (int i = 0; i < operationCods_RemovingBuff.Count(); i++)
                    //{
                    //    Status = operationCods.Remove(operationCods_RemovingBuff[i]);
                    //}
                    /////////////-----
                    short count = 0;
                        for (int p = 0; p < operationCods_RemovingBuff.Count(); p++)
                        {
                            count = 0;
                            for (int h = 0; h < operationCods.Count(); h++)
                            {
                                if (operationCods[h].AdditionalParam_DirPath == operationCods_RemovingBuff[p].AdditionalParam_DirPath)
                                    count++;
                                if (operationCods[h].AdditionalParam_NumRemotePC == operationCods_RemovingBuff[p].AdditionalParam_NumRemotePC)
                                    count++;
                                if (operationCods[h].ChainPartNumber == operationCods_RemovingBuff[p].ChainPartNumber)
                                    count++;
                                if (operationCods[h].CodeNumberInPart == operationCods_RemovingBuff[p].CodeNumberInPart)
                                    count++;
                                if (operationCods[h].IsChain == operationCods_RemovingBuff[p].IsChain)
                                    count++;
                                if (operationCods[h].NumberLinkedPC == operationCods_RemovingBuff[p].NumberLinkedPC)
                                    count++;
                                if (operationCods[h].NumberPC == operationCods_RemovingBuff[p].NumberPC)
                                    count++;
                                if (operationCods[h].OperationNumber == operationCods_RemovingBuff[p].OperationNumber)
                                    count++;
                                if (operationCods[h].OperationStatus == operationCods_RemovingBuff[p].OperationStatus)
                                    count++;
                                if (operationCods[h].OperationTypes[0] == operationCods_RemovingBuff[p].OperationTypes[0] &&
                                    operationCods[h].OperationTypes[1] == operationCods_RemovingBuff[p].OperationTypes[1])
                                    count++;
                                if (operationCods[h].TotalCodesInPart == operationCods_RemovingBuff[p].TotalCodesInPart)
                                    count++;
                                if (operationCods[h].TotalPartsInChain == operationCods_RemovingBuff[p].TotalPartsInChain)
                                    count++;

                                if (count == 12)
                                {
                                    operationCods.RemoveAt(h);
                                    break;
                                }
                            }
                        }
                        operationCods_RemovingBuff.Clear();
                }
                OpSem.Release();
                Thread.Sleep(ControlThreadSleepingTime);
            }
        }
        internal void СonfirmOperationCod(OperationCod operationCod)
        {
            OpSem.WaitOne();

            List<OperationCod> cods = new List<OperationCod>(operationCods);
            operationCods_RemovingBuff.Add(operationCod);
            if (operationCod.IsChain == true)
            {
                if (operationCod.CodeNumberInPart < operationCod.TotalCodesInPart)
                {
                    short countVerificated;
                    for (int i = 0; i < cods.Count(); i++)
                    {
                        countVerificated = 0;
                        if (cods[i].NumberPC == operationCod.NumberLinkedPC ||
                            cods[i].NumberLinkedPC == operationCod.NumberPC)
                            countVerificated++;
                        if (cods[i].IsChain == operationCod.IsChain)
                            countVerificated++;
                        if (cods[i].OperationTypes == operationCod.OperationTypes)
                            countVerificated++;
                        if (cods[i].OperationStatus == -1)
                            countVerificated++;
                        if (cods[i].ChainPartNumber == operationCod.ChainPartNumber)
                            countVerificated++;
                        if (cods[i].TotalCodesInPart == operationCod.TotalCodesInPart)
                            countVerificated++;
                        if (cods[i].TotalPartsInChain == operationCod.TotalPartsInChain)
                            countVerificated++;
                        if (cods[i].CodeNumberInPart == (operationCod.CodeNumberInPart + 1))
                            countVerificated++;
                        if (countVerificated == 8)
                        {
                            cods[i].OperationStatus = 1;
                            if (RunOperationCode != null) RunOperationCode(cods[i], EventArgs.Empty);
                            break;
                        }//след. коп из части цепочки для запуска нейден. Запуск.
                    }
                }//не последний коп в части цепочки
                else if (operationCod.CodeNumberInPart == operationCod.TotalCodesInPart &&
                    operationCod.ChainPartNumber < operationCod.TotalPartsInChain)
                {
                    short countVerificated;
                    for (int i = 0; i < cods.Count(); i++)
                    {
                        countVerificated = 0;
                        if (cods[i].NumberPC == operationCod.NumberLinkedPC ||
                            cods[i].NumberLinkedPC == operationCod.NumberPC)
                            countVerificated++;
                        if (cods[i].IsChain == operationCod.IsChain)
                            countVerificated++;
                        if ((cods[i].OperationTypes[0] == operationCod.OperationTypes[0] &&
                            cods[i].OperationTypes[1] == operationCod.OperationTypes[1]) ||
                            (operationCods[i].OperationTypes[0] == operationCod.OperationTypes[1] &&
                            cods[i].OperationTypes[1] == operationCod.OperationTypes[0]))
                            countVerificated++;
                        if (cods[i].OperationStatus == -1)
                            countVerificated++;
                        if (cods[i].ChainPartNumber == (operationCod.ChainPartNumber + 1))
                            countVerificated++;
                        if (cods[i].TotalPartsInChain == operationCod.TotalPartsInChain)
                            countVerificated++;
                        if (cods[i].CodeNumberInPart == 0)
                            countVerificated++;
                        if (countVerificated == 7)
                        {
                            UnlockForbiddenTypes(operationCod);
                            cods[i].OperationStatus = 0;
                            break;
                        }//найден последний коп в часте цепочки.
                    }//поиск первого коп след части чепочки
                }//последний коп в не последней части цепочки
                else if(operationCod.ChainPartNumber == operationCod.TotalPartsInChain &&
                    operationCod.CodeNumberInPart == operationCod.TotalCodesInPart)
                {
                    UnlockForbiddenTypes(operationCod);
                }//последний коп в последней части цепочки
            }//КОП относится к цепочке
            else
            {
                UnlockForbiddenTypes(operationCod);
                //operationCods_RemovingBuff.Add(operationCod);
            }//КОП не относится к цепочке  

            OpSem.Release();
        }
        private void UnlockForbiddenTypes(OperationCod operationCod)
        {
            for (int j = 0; j < operationFlags.Count(); j++)
            {
                if (operationFlags[j].NumberPC == operationCod.NumberPC)
                {
                    foreach (UInt16 type in operationCod.OperationTypes)
                    {
                        operationFlags[j].ForbiddenTypes.Remove(type);
                    }
                }
                if (operationCod.NumberPC != operationCod.NumberLinkedPC && operationCod.IsChain == true)
                {
                    if (operationFlags[j].NumberPC == operationCod.NumberLinkedPC)
                    {
                        foreach (UInt16 type in operationCod.OperationTypes)
                        {
                            operationFlags[j].ForbiddenTypes.Remove(type);
                        }
                    }
                }
            }//поиск объектов флагов для 2х связанных ПК или одного, если поля в коп "номер пк" и "номер связанного пк" равны
        }//удаление из объекта / объектов флагов запрещенных типов
    }
}

