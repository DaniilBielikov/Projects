using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;

namespace Kursach_Uchet_Internet_Uslug
{
    /// <summary>
    /// Служба содержит методы редактирования записей в БД.
    /// Формат входных данных каждой функции - строка.
    /// Строка имеет формат: <Поле0> "$" <Поле1> "$" <Поле2>
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Edit : System.Web.Services.WebService
    {

        [WebMethod]
        public void Edit_Gorod(string EditGorod)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditGorod.Split('$');

            var gorod = context.Goroda.Where(p => p.Id_Goroda == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (gorod != null)
            {
                gorod.Nazvanie = splited_string[1];
                context.Goroda.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Rayon(string EditRayon)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditRayon.Split('$');

            var rayon = context.Rayoni.Where(p => p.Id_Rayona == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (rayon != null)
            {
                rayon.Id_Goroda = Convert.ToInt32(splited_string[1]);
                rayon.Nazvanie = splited_string[2];
                context.Rayoni.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Uliza(string EditUliza)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditUliza.Split('$');

            var uliza = context.Ulizi.Where(p => p.Id_Ulizi == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (uliza != null)
            {
                uliza.Id_Rayona = Convert.ToInt32(splited_string[1]);
                uliza.Nazvanie = splited_string[2];
                context.Ulizi.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Adress(string EditAdress)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditAdress.Split('$');

            var adress = context.Adresa.Where(p => p.Id_Adresa == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (adress != null)
            {
                adress.Id_Ulizi = Convert.ToInt32(splited_string[1]);
                adress.Nomer_doma = splited_string[2];
                adress.Nomer_Kvartiri = splited_string[3];
                context.Adresa.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Klienta(string EditKlienta)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditKlienta.Split('$');

            var klient = context.Klienti.Where(p => p.Id_Klienta == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (klient != null)
            {
                klient.Familiya = splited_string[1];
                klient.Imya = splited_string[2];
                klient.Otchestvo = splited_string[3];
                klient.Telefon = splited_string[4];
                klient.INN = splited_string[5];
                klient.Pasport = splited_string[6];
                klient.Id_Adresa = Convert.ToInt32(splited_string[7]);
                klient.Id_Scheta = Convert.ToInt32(splited_string[8]);
                klient.Data_podklucheniya = DateTime.ParseExact(splited_string[9], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                if (splited_string[10] == "")
                    klient.Data_otklucheniya = null;
                else
                    klient.Data_otklucheniya = DateTime.ParseExact(splited_string[10], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                context.Adresa.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Schet_Klienta(string EditSchet_Klienta)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditSchet_Klienta.Split('$');

            var schet_klienta = context.Scheta.Where(p => p.Id_Scheta == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (schet_klienta != null)
            {
                schet_klienta.Summa = Convert.ToInt32(splited_string[1]);
                context.Scheta.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Doljnost(string EditDoljnost)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditDoljnost.Split('$');

            var doljnost = context.Doljnosti.Where(p => p.Id_Doljnosti == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (doljnost != null)
            {
                doljnost.Nazvanie = splited_string[1];
                doljnost.Oklad = Convert.ToInt32(splited_string[2]);
                doljnost.Opisanie = splited_string[3];
                context.Doljnosti.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Sotrudnika(string EditSotrudnika)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditSotrudnika.Split('$');

            var sotrudnik = context.Sotrudniki.Where(p => p.Id_Sotrudnika == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (sotrudnik != null)
            {
                sotrudnik.Familiya = splited_string[1];
                sotrudnik.Imya = splited_string[2];
                sotrudnik.Otchestvo = splited_string[3];
                sotrudnik.Telefon = splited_string[4];
                sotrudnik.E_mail = splited_string[5];
                sotrudnik.INN = splited_string[6];
                sotrudnik.Pasport = splited_string[7];
                sotrudnik.Date_reg = DateTime.ParseExact(splited_string[8], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                context.Sotrudniki.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Sotrudnik_Doljnost(string EditSotrudnik_Doljnost)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditSotrudnik_Doljnost.Split('$');

            var sotrudnik_doljnost = context.Sotrudniki_Doljnosti.Where(p => p.Id_Sotrudniki_Doljnosti == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (sotrudnik_doljnost != null)
            {
                sotrudnik_doljnost.Id_Sotrudnika = Convert.ToInt32(splited_string[1]);
                sotrudnik_doljnost.Id_Doljnosti = Convert.ToInt32(splited_string[2]);
                sotrudnik_doljnost.Data_s = DateTime.ParseExact(splited_string[3], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                if (splited_string[4] == "")
                    sotrudnik_doljnost.Data_po = null;
                else
                    sotrudnik_doljnost.Data_po = DateTime.ParseExact(splited_string[4], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                context.Sotrudniki_Doljnosti.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Uslugu(string EditUslugu)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditUslugu.Split('$');

            var usluga = context.Uslugi.Where(p => p.Id_Uslugi == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (usluga != null)
            {
                usluga.Nazvanie = splited_string[1];
                usluga.Opisanie = splited_string[2];
                usluga.Zena = Convert.ToInt32(splited_string[3]);
                usluga.Aktualna = splited_string[4];
                usluga.Data_s = DateTime.ParseExact(splited_string[5], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                if (splited_string[6] == "")
                    usluga.Data_po = null;
                else
                    usluga.Data_po = DateTime.ParseExact(splited_string[6], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                context.Uslugi.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_Klient_Usluga(string EditKlient_Usluga)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditKlient_Usluga.Split('$');

            var klient_usluga = context.Klient_Uslugi.Where(p => p.Id_Klient_Uslugi == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (klient_usluga != null)
            {
                klient_usluga.Id_Sotrudnika = Convert.ToInt32(splited_string[1]);
                klient_usluga.Id_Klienta = Convert.ToInt32(splited_string[2]);
                klient_usluga.Id_Uslugi = Convert.ToInt32(splited_string[3]);
                klient_usluga.Dara_nachala = DateTime.ParseExact(splited_string[4], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                if (splited_string[5] == "")
                    klient_usluga.Data_konza = null;
                else
                    klient_usluga.Data_konza = DateTime.ParseExact(splited_string[5], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                context.Klient_Uslugi.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Edit_IstoriyaOplati(string EditIstoriyaOplati)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = EditIstoriyaOplati.Split('$');

            var istoriya_oplati = context.Istoriya_Oplati.Where(p => p.Id_Istorii_oplati == Convert.ToInt32(splited_string[0])).FirstOrDefault();
            if (istoriya_oplati != null)
            {
                istoriya_oplati.Id_Klient_Uslugi = Convert.ToInt32(splited_string[1]);
                istoriya_oplati.Summa = Convert.ToInt32(splited_string[2]);
                istoriya_oplati.Data_oplati = DateTime.ParseExact(splited_string[3], "yyyy-MM-dd", CultureInfo.CurrentCulture);
                istoriya_oplati.Resultat_Oplati = splited_string[4];
                context.Istoriya_Oplati.Context.SubmitChanges();
            }
        }
    }
}
