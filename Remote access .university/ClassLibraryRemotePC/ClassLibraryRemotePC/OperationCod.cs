using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ClassLibraryRemotePC
{
    /// <summary>
    /// Класс предоставляющий механизмы управлением работой нескольких компьютеров. Сериализуемый.
    /// </summary>
    [Serializable]
    public class OperationCod
    {
        private byte[] _data;
        /// <summary>
        /// Сериализированный объект OperationCod.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        #region КОП (переменные)
        public Int16 NumberPC { set; get; } //-1 this PC! >-1 Remote PCs!
        public List<UInt16> OperationTypes { set; get; } 
        //Первым в списке всегда должен быть тип операции, номер которой указанный в поле operationNumber,
        // дальше могут идти связанные с данным типом типы операций.
        public UInt16 OperationNumber { set; get; }
        public Int16 OperationStatus { set; get; }
        //-1 - заблоктрован; 0-не установлена на выполнеие;  1-установлена на выполнение (ожидает подтверждения);
        public bool IsChain { set; get; }
        public Int16 NumberLinkedPC { set; get; }
        public Int16 ChainPartNumber { set; get; }
        public Int16 TotalPartsInChain { set; get; }
        public Int16 CodeNumberInPart { set; get; }
        public Int16 TotalCodesInPart { set; get; }

        #endregion

        #region Дополнительные параметры (переменные)
        public string AdditionalParam_DirPath { set; get; }
        public short AdditionalParam_NumRemotePC { set; get; }
        public string AdditionalParam_DirPath2 { set; get; }
        #endregion

        public OperationCod() { }

        /// <summary>
        /// Создает объект OperationCod.
        /// </summary>
        /// <param name="pcNumber">Номер ПК, котрый должен выполнить операцию предписанную в КОП.</param>
        /// <param name="operationTypes">Тип операции. Первым в списке всегда идет номер типа к которому
        /// относится параметр "operationNumber"</param>
        /// <param name="operationNumber">Номер операции в пределах типа операции.</param>
        /// <param name="operationStatus">Статус операции.
        /// -1 - заблокирована; 0 - не установлена на выполнение;
        /// 1 - установлена на выполнение (ожидает подтверждения).</param>
        /// <param name="isChain">Цепочка КОП или нет. Если false, следующие параметры можно указать любыми.</param>
        /// <param name="numberLinkedPC">Номер ПК с которым связана цепочка. Может быть тот же номер что и в поле "pcNumber".</param>
        /// <param name="chainPartNumber">Номер части цепочки.</param>
        /// <param name="totalPartsInChain">Общее количество частей в цепочке. Счет от 0!!!</param>
        /// <param name="codeNumberInPart">Номер КОП в части цепочки.</param>
        /// <param name="totalCodesInPart">Обшее количество КОП внутри части цепочки. Счет от 0!!!</param>
        public OperationCod(Int16 pcNumber, List<UInt16> operationTypes, UInt16 operationNumber, Int16 operationStatus,
             bool isChain, Int16 numberLinkedPC, Int16 chainPartNumber, Int16 totalPartsInChain, Int16 codeNumberInPart,
             Int16 totalCodesInPart)
        {
            NumberPC = pcNumber;
            OperationTypes = operationTypes;
            OperationNumber = operationNumber;
            OperationStatus = operationStatus;
            IsChain = isChain;
            NumberLinkedPC = numberLinkedPC;
            ChainPartNumber = chainPartNumber;
            TotalPartsInChain = totalPartsInChain;
            CodeNumberInPart = codeNumberInPart;
            TotalCodesInPart = totalCodesInPart;
        }

        /// <summary>
        /// Создает OperationCod на основе массива байт.
        /// </summary>
        /// <param name="data">Массив байт с сериализованным объектом OperationCod</param>
        public OperationCod(byte[] data)
        {
            OperationCod OpCod = FromArray(data);
            Data = OpCod.Data;
            NumberPC = OpCod.NumberPC;
            OperationTypes = OpCod.OperationTypes;
            OperationNumber = OpCod.OperationNumber;
            OperationStatus = OpCod.OperationStatus;
            IsChain = OpCod.IsChain;
            NumberLinkedPC = OpCod.NumberLinkedPC;
            ChainPartNumber = OpCod.ChainPartNumber;
            TotalPartsInChain = OpCod.TotalPartsInChain;
            CodeNumberInPart = OpCod.CodeNumberInPart;
            TotalCodesInPart = OpCod.TotalCodesInPart;
            AdditionalParam_DirPath = OpCod.AdditionalParam_DirPath;
            AdditionalParam_DirPath2 = OpCod.AdditionalParam_DirPath2;
            AdditionalParam_NumRemotePC = OpCod.AdditionalParam_NumRemotePC;
        }

        /// <summary>
        /// Сериализирует объект OperationCod в массив байт.
        /// </summary>
        /// <returns>Массив байт.</returns>
        public byte[] ToArray()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);

                return stream.ToArray();
            }
        }
        /// <summary>
        /// Десериализирует объект OperationCod из массива байт.
        /// </summary>
        /// <param name="data">Массив байт.</param>
        /// <returns>Объект OperationCod полученный в результате десериализации.</returns>
        public  OperationCod FromArray(byte[] data)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream(data))
                {
                    stream.Position = 0;
                    return (OperationCod)formatter.Deserialize(stream);
                }
            }
            catch
            {
                OperationCod cod = new OperationCod();
                return cod;
            }
        }
    }
}
