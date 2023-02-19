using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using System.Data.Linq;
using System.Globalization;

namespace Kursach_Uchet_Internet_Uslug
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "TestService" в коде и файле конфигурации.
    public class TestService : ITestService
    {
        public void IstoriyaIzmeneniyaZenUsluga(DateTime Start_date, string NameUslugi)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            SqlConnection connection = new SqlConnection(context.Connection.ConnectionString);


            SqlCommand command = new SqlCommand(@"select usl.Id_Uslugi, usl.Nazvanie, usl.Data_s, usl.Data_po, usl.Zena
                                            from uslugi usl
                                            where (usl.Data_s) > @Start_date and ltrim(rtrim(usl.Nazvanie)) = @NameUslugi
                                            order by usl.Nazvanie", connection);

            command.Parameters.Add("@Start_date", SqlDbType.DateTime);
            command.Parameters["@Start_date"].Value = Start_date;

            command.Parameters.Add("@NameUslugi", SqlDbType.VarChar);
            command.Parameters["@NameUslugi"].Value = NameUslugi;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<string> result = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull(3) == true)
                    {
                        result.Add(reader.GetInt32(0) + "$" + reader.GetString(1) + "$" + reader.GetDateTime(2)
                        + "$" + "-" + "$" + reader.GetInt32(4));
                    }
                    else
                    {
                        result.Add(reader.GetInt32(0) + "$" + reader.GetString(1) + "$" + reader.GetDateTime(2)
                            + "$" + reader.GetDateTime(3) + "$" + reader.GetInt32(4));
                    }
                }
            }
            OperationContext.Current.GetCallbackChannel<ITestServiceCallback>().SendResult(result);
        }
        public void OpredeleniePopularnostiUslug()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            SqlConnection connection = new SqlConnection(context.Connection.ConnectionString);


            SqlCommand command = new SqlCommand(@"select count(klu.id_uslugi), usl.Nazvanie
                                            from uslugi usl
                                            inner join Klient_Uslugi klu on usl.Id_Uslugi = klu.Id_Uslugi 
                                            group by usl.Nazvanie
                                            order by usl.Nazvanie desc", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<string> result = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(reader.GetInt32(0) + "$" + reader.GetString(1));
                }
            }
            OperationContext.Current.GetCallbackChannel<ITestServiceCallback>().SendResult(result);
        }
        public void KarerniyRostSotrudnika(int Id_Sotrudnika)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            SqlConnection connection = new SqlConnection(context.Connection.ConnectionString);


            SqlCommand command = new SqlCommand(@"select sotr.Id_Sotrudnika, sotr.Familiya, sotr.Imya, sotr.Otchestvo, dol.Nazvanie, dol.Oklad, sotrd.Data_s, sotrd.Data_po
                                        from Sotrudniki sotr 
                                        inner join Sotrudniki_Doljnosti sotrd on sotrd.Id_Sotrudnika= sotr.Id_Sotrudnika
                                        inner join Doljnosti dol on dol.Id_Doljnosti = sotrd.Id_Doljnosti
                                        where sotr.Id_Sotrudnika = @Id_Sotrudnika
                                        order by sotr.Id_Sotrudnika, sotrd.Data_s desc", connection);

            command.Parameters.Add("@Id_Sotrudnika", SqlDbType.Int);
            command.Parameters["@Id_Sotrudnika"].Value = Id_Sotrudnika;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<string> result = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull(7) == true)
                    {
                        result.Add(reader.GetInt32(0) + "$" + reader.GetString(1) + "$" + reader.GetString(2)
                        + "$" + reader.GetString(3) + "$" + reader.GetString(4) + "$" + reader.GetInt32(5)
                        + "$" + reader.GetDateTime(6) + "$" + "-");
                    }
                    else
                    {
                        result.Add(reader.GetInt32(0) + "$" + reader.GetString(1) + "$" + reader.GetString(2)
                        + "$" + reader.GetString(3) + "$" + reader.GetString(4) + "$" + reader.GetInt32(5)
                        + "$" + reader.GetDateTime(6) + "$" + reader.GetDateTime(7));
                    }
                }
            }
            OperationContext.Current.GetCallbackChannel<ITestServiceCallback>().SendResult(result);
        }
        public void KolvoPodkluchOtkluchPolsovateley()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            SqlConnection connection = new SqlConnection(context.Connection.ConnectionString);


            SqlCommand command = new SqlCommand(@"select count(kl.Id_Klienta)--, kl.Data_otklucheniya
                                        from klienti kl
                                        where  kl.Data_otklucheniya is null
                                        group by kl.Data_otklucheniya
                                        union
                                        select count (kl.id_klienta)
                                        from klienti kl
                                        where kl.Data_otklucheniya is not null
                                        group by kl.Data_podklucheniya", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<string> result = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(reader.GetInt32(0).ToString());
                }
            }
            OperationContext.Current.GetCallbackChannel<ITestServiceCallback>().SendResult(result);
        }
    }
}
