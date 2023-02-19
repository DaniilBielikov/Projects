using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;

namespace Kursach_Uchet_Internet_Uslug
{
    /// <summary>
    /// Служба содержит функции вывода каждой таблицы БД.
    /// Формат резултата каждой функции - список строк.
    /// Каждая строка в списке имеет формат: <Поле0> "$" <Поле1> "$" <Поле2>
    /// Знак доллара является разделителем полей.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Show : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> Show_Goroda()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            //SqlConnection connection = new SqlConnection(context.Connection.ConnectionString);

            List<string> str = new List<string>();
            var goroda = context.Goroda.ToList();
            foreach (var gorod in goroda)
            {
                str.Add( gorod.Id_Goroda + "$" + gorod.Nazvanie);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Rayoni()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var rayoni = context.Rayoni.ToList();
            foreach (var rayon in rayoni)
            {
                str.Add(rayon.Id_Rayona + "$" + rayon.Id_Goroda + "$" + rayon.Nazvanie);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Ulizi()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var ulizi = context.Ulizi.ToList();
            foreach (var uliza in ulizi)
            {
                str.Add(uliza.Id_Ulizi + "$" + uliza.Id_Rayona + "$" + uliza.Nazvanie);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Adresa()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var adresa = context.Adresa.ToList();
            foreach (var adres in adresa)
            {
                str.Add(adres.Id_Adresa + "$" + adres.Id_Ulizi + "$" + adres.Nomer_doma + "$" + adres.Nomer_Kvartiri);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Klienti()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var klienti = context.Klienti.ToList();
            foreach (var klient in klienti)
            {
                str.Add(klient.Id_Klienta + "$" + klient.Familiya + "$" + klient.Imya + "$" + klient.Otchestvo + "$" +
                    klient.Telefon + "$" + klient.INN + "$" + klient.Pasport + "$" + klient.Id_Adresa + "$" + 
                    klient.Id_Scheta + "$" + klient.Data_podklucheniya + "$" + klient.Data_otklucheniya);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Scheta_Klientov()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var scheta_klientov = context.Scheta.ToList();
            foreach (var schet_klienta in scheta_klientov)
            {
                str.Add(schet_klienta.Id_Scheta + "$" + schet_klienta.Summa);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Doljnosti()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var doljnosti = context.Doljnosti.ToList();
            foreach (var doljnost in doljnosti)
            {
                str.Add(doljnost.Id_Doljnosti + "$" + doljnost.Nazvanie + "$" + doljnost.Oklad + "$" + doljnost.Opisanie);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Sotrudniki()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var sotrudniki = context.Sotrudniki.ToList();
            foreach (var sotrudnik in sotrudniki)
            {
                str.Add(sotrudnik.Id_Sotrudnika + "$" + sotrudnik.Familiya + "$" + sotrudnik.Imya + "$" + 
                    sotrudnik.Otchestvo + "$" + sotrudnik.Telefon + "$" + sotrudnik.E_mail + "$" + sotrudnik.INN + "$" + 
                    sotrudnik.Pasport + "$" + sotrudnik.Date_reg);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Sotrudniki_Doljnosti()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var sotrudniki_doljnosti = context.Sotrudniki_Doljnosti.ToList();
            foreach (var sotrudnik_doljnost in sotrudniki_doljnosti)
            {
                str.Add(sotrudnik_doljnost.Id_Sotrudniki_Doljnosti + "$" + sotrudnik_doljnost.Id_Sotrudnika + "$" + 
                    sotrudnik_doljnost.Id_Doljnosti + "$" + sotrudnik_doljnost.Data_s + "$" + sotrudnik_doljnost.Data_po);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Uslugi()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var uslugi = context.Uslugi.ToList();
            foreach (var usluga in uslugi)
            {
                str.Add(usluga.Id_Uslugi + "$" + usluga.Nazvanie + "$" + usluga.Opisanie + "$" + usluga.Zena + "$" + 
                    usluga.Aktualna + "$" + usluga.Data_s + "$" + usluga.Data_po);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Klient_Uslugi()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var klient_uslugi = context.Klient_Uslugi.ToList();
            foreach (var klient_usluga in klient_uslugi)
            {
                str.Add(klient_usluga.Id_Klient_Uslugi + "$" + klient_usluga.Id_Sotrudnika + "$" + klient_usluga.Id_Klienta + "$" + 
                    klient_usluga.Id_Uslugi + "$" + klient_usluga.Dara_nachala + "$" + klient_usluga.Data_konza);
            }
            return str;
        }

        [WebMethod]
        public List<string> Show_Istoriya_Oplati()
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();

            List<string> str = new List<string>();
            var istoriya_oplati = context.Istoriya_Oplati.ToList();
            foreach (var odna_istoriya_oplati in istoriya_oplati)
            {
                str.Add(odna_istoriya_oplati.Id_Istorii_oplati + "$" + odna_istoriya_oplati.Id_Klient_Uslugi + "$" + 
                    odna_istoriya_oplati.Summa + "$" + odna_istoriya_oplati.Data_oplati + "$" + odna_istoriya_oplati.Resultat_Oplati);
            }
            return str;
        }
    }
}
