using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Kursach_Uchet_Internet_Uslug
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "ITestService" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(ITestServiceCallback))]
    public interface ITestService
    {
        [OperationContract(IsOneWay = true)]
        void IstoriyaIzmeneniyaZenUsluga(DateTime Start_date, string NameUslugi);
        [OperationContract(IsOneWay = true)]
        void OpredeleniePopularnostiUslug();
        [OperationContract(IsOneWay = true)]
        void KarerniyRostSotrudnika(int Id_Sotrudnika);
        [OperationContract(IsOneWay = true)]
        void KolvoPodkluchOtkluchPolsovateley();
    }

    public interface ITestServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendResult(List<string> res);
    }

}
