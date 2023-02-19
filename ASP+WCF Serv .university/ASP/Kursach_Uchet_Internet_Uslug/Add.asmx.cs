using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Data.SqlTypes;

namespace Kursach_Uchet_Internet_Uslug
{
    /// <summary>
    /// Служба содержит методы добавления записей в БД.
    /// Формат входных данных каждой функции - строка.
    /// Строка имеет формат: <Поле0> "$" <Поле1> "$" <Поле2>
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Add : System.Web.Services.WebService
    {

        [WebMethod]
        public void Add_Gorod(string AddGorod)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddGorod.Split('$');

            var newGorod = new Goroda
            {
                Id_Goroda = Convert.ToInt32(splited_string[0]),
                Nazvanie = splited_string[1]
            };
            context.Goroda.InsertOnSubmit(newGorod);
            context.Goroda.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Rayon(string AddRayon)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddRayon.Split('$');

            var newRayon = new Rayoni
            {
                Id_Rayona = Convert.ToInt32(splited_string[0]),
                Id_Goroda = Convert.ToInt32(splited_string[1]),
                Nazvanie = splited_string[2]
            };
            context.Rayoni.InsertOnSubmit(newRayon);
            context.Rayoni.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Ulizu(string AddUlizu)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddUlizu.Split('$');

            var newUliza = new Ulizi
            {
                Id_Ulizi = Convert.ToInt32(splited_string[0]),
                Id_Rayona = Convert.ToInt32(splited_string[1]),
                Nazvanie = splited_string[2]
            };
            context.Ulizi.InsertOnSubmit(newUliza);
            context.Ulizi.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Adres(string AddAdres)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddAdres.Split('$');

            var newAdres = new Adresa
            {
                Id_Adresa = Convert.ToInt32(splited_string[0]),
                Id_Ulizi = Convert.ToInt32(splited_string[1]),
                Nomer_doma = splited_string[2],
                Nomer_Kvartiri = splited_string[3]
            };
            context.Adresa.InsertOnSubmit(newAdres);
            context.Adresa.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Klienta(string AddKlienta)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddKlienta.Split('$');

            if (splited_string[10] == "")
            {
                var newKlient = new Klienti
                {
                    Id_Klienta = Convert.ToInt32(splited_string[0]),
                    Familiya = splited_string[1],
                    Imya = splited_string[2],
                    Otchestvo = splited_string[3],
                    Telefon = splited_string[4],
                    INN = splited_string[5],
                    Pasport = splited_string[6],
                    Id_Adresa = Convert.ToInt32(splited_string[7]),
                    Id_Scheta = Convert.ToInt32(splited_string[8]),
                    Data_podklucheniya = DateTime.ParseExact(splited_string[9], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_otklucheniya = null
                };
                context.Klienti.InsertOnSubmit(newKlient);
            }
            else
            {
                var newKlient = new Klienti
                {
                    Id_Klienta = Convert.ToInt32(splited_string[0]),
                    Familiya = splited_string[1],
                    Imya = splited_string[2],
                    Otchestvo = splited_string[3],
                    Telefon = splited_string[4],
                    INN = splited_string[5],
                    Pasport = splited_string[6],
                    Id_Adresa = Convert.ToInt32(splited_string[7]),
                    Id_Scheta = Convert.ToInt32(splited_string[8]),
                    Data_podklucheniya = DateTime.ParseExact(splited_string[9], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_otklucheniya = DateTime.ParseExact(splited_string[10], "yyyy-MM-dd", CultureInfo.CurrentCulture)
                };
                context.Klienti.InsertOnSubmit(newKlient);
            }
            context.Klienti.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_SchetKlienta(string AddSchetKlienta)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddSchetKlienta.Split('$');

            var newSchetKlienta = new Scheta
            {
                Id_Scheta = Convert.ToInt32(splited_string[0]),
                Summa = Convert.ToInt32(splited_string[1])
            };
            context.Scheta.InsertOnSubmit(newSchetKlienta);
            context.Scheta.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Doljnost(string AddDoljnost)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddDoljnost.Split('$');

            var newDoljnost = new Doljnosti
            {
                Id_Doljnosti = Convert.ToInt32(splited_string[0]),
                Nazvanie = splited_string[1],
                Oklad = Convert.ToInt32(splited_string[2]),
                Opisanie = splited_string[3]
            };
            context.Doljnosti.InsertOnSubmit(newDoljnost);
            context.Doljnosti.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Sotrudnika(string AddSotrudnika)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddSotrudnika.Split('$');

            var newSotrudnik = new Sotrudniki
            {
                Id_Sotrudnika = Convert.ToInt32(splited_string[0]),
                Familiya = splited_string[1],
                Imya = splited_string[2],
                Otchestvo = splited_string[3],
                Telefon = splited_string[4],
                E_mail = splited_string[5],
                INN = splited_string[6],
                Pasport = splited_string[7],
                Date_reg = DateTime.ParseExact(splited_string[8], "yyyy-MM-dd", CultureInfo.CurrentCulture)
            };
            context.Sotrudniki.InsertOnSubmit(newSotrudnik);
            context.Sotrudniki.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Sotrudnik_Doljnost(string AddSotrudnik_Doljnost)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddSotrudnik_Doljnost.Split('$');

            if (splited_string[4] == "")
            {
                var newSotrudnik_Doljnost = new Sotrudniki_Doljnosti
                {
                    Id_Sotrudniki_Doljnosti = Convert.ToInt32(splited_string[0]),
                    Id_Sotrudnika = Convert.ToInt32(splited_string[1]),
                    Id_Doljnosti = Convert.ToInt32(splited_string[2]),
                    Data_s = DateTime.ParseExact(splited_string[3], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_po = null
                };
                context.Sotrudniki_Doljnosti.InsertOnSubmit(newSotrudnik_Doljnost);
            }
            else
            {
                var newSotrudnik_Doljnost = new Sotrudniki_Doljnosti
                {
                    Id_Sotrudniki_Doljnosti = Convert.ToInt32(splited_string[0]),
                    Id_Sotrudnika = Convert.ToInt32(splited_string[1]),
                    Id_Doljnosti = Convert.ToInt32(splited_string[2]),
                    Data_s = DateTime.ParseExact(splited_string[3], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_po = DateTime.ParseExact(splited_string[4], "yyyy-MM-dd", CultureInfo.CurrentCulture)
                };
                context.Sotrudniki_Doljnosti.InsertOnSubmit(newSotrudnik_Doljnost);
            }
            context.Sotrudniki_Doljnosti.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_Uslugu(string AddUslugu)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddUslugu.Split('$');

            if (splited_string[6] == "")
            {
                var newUsluga = new Uslugi
                {
                    Id_Uslugi = Convert.ToInt32(splited_string[0]),
                    Nazvanie = splited_string[1],
                    Opisanie = splited_string[2],
                    Zena = Convert.ToInt32(splited_string[3]),
                    Aktualna = splited_string[4],
                    Data_s = DateTime.ParseExact(splited_string[5], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_po = null
                };
                context.Uslugi.InsertOnSubmit(newUsluga);
            }
            else
            {
                var newUsluga = new Uslugi
                {
                    Id_Uslugi = Convert.ToInt32(splited_string[0]),
                    Nazvanie = splited_string[1],
                    Opisanie = splited_string[2],
                    Zena = Convert.ToInt32(splited_string[3]),
                    Aktualna = splited_string[4],
                    Data_s = DateTime.ParseExact(splited_string[5], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_po = DateTime.ParseExact(splited_string[6], "yyyy-MM-dd", CultureInfo.CurrentCulture)
                };
                context.Uslugi.InsertOnSubmit(newUsluga);
            }
            context.Uslugi.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_KlientUsluga(string AddKlientUsluga)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddKlientUsluga.Split('$');

            if (splited_string[5] == "")
            {
                var newKlientUsluga = new Klient_Uslugi
                {
                    Id_Klient_Uslugi = Convert.ToInt32(splited_string[0]),
                    Id_Sotrudnika = Convert.ToInt32(splited_string[1]),
                    Id_Klienta = Convert.ToInt32(splited_string[2]),
                    Id_Uslugi = Convert.ToInt32(splited_string[3]),
                    Dara_nachala = DateTime.ParseExact(splited_string[4], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_konza = null
                };
                context.Klient_Uslugi.InsertOnSubmit(newKlientUsluga);
            }
            else
            {
                var newKlientUsluga = new Klient_Uslugi
                {
                    Id_Klient_Uslugi = Convert.ToInt32(splited_string[0]),
                    Id_Sotrudnika = Convert.ToInt32(splited_string[1]),
                    Id_Klienta = Convert.ToInt32(splited_string[2]),
                    Id_Uslugi = Convert.ToInt32(splited_string[3]),
                    Dara_nachala = DateTime.ParseExact(splited_string[4], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                    Data_konza = DateTime.ParseExact(splited_string[5], "yyyy-MM-dd", CultureInfo.CurrentCulture)
                };
                context.Klient_Uslugi.InsertOnSubmit(newKlientUsluga);
            }
            context.Klient_Uslugi.Context.SubmitChanges();
        }

        [WebMethod]
        public void Add_IstoriyuOplati(string AddIstoriyuOplati)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            string[] splited_string;
            splited_string = AddIstoriyuOplati.Split('$');

            var newIstoriyuOplati = new Istoriya_Oplati
            {
                Id_Istorii_oplati = Convert.ToInt32(splited_string[0]),
                Id_Klient_Uslugi = Convert.ToInt32(splited_string[1]),
                Summa = Convert.ToInt32(splited_string[2]),
                Data_oplati = DateTime.ParseExact(splited_string[3], "yyyy-MM-dd", CultureInfo.CurrentCulture),
                Resultat_Oplati = splited_string[4]
            };
            context.Istoriya_Oplati.InsertOnSubmit(newIstoriyuOplati);
            context.Istoriya_Oplati.Context.SubmitChanges();
        }
    }
}
