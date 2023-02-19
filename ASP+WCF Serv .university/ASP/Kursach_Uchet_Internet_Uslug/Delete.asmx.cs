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
    /// Служба содержит методы удаления записей в БД.
    /// Формат входных данных каждой функции - строка.
    /// Строка должна содержать только значение ключевого поля (Id...) записи таблицы.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Delete : System.Web.Services.WebService
    {

        [WebMethod]
        public void Delete_Gorod(string IdDeleteGorod)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var gorod = context.Goroda.Where(p => p.Id_Goroda == Convert.ToInt32(IdDeleteGorod)).FirstOrDefault();
            if (gorod != null)
            {
                context.Goroda.DeleteOnSubmit(gorod);
                context.Goroda.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Rayon(string IdDeleteRayon)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var rayon = context.Rayoni.Where(p => p.Id_Rayona == Convert.ToInt32(IdDeleteRayon)).FirstOrDefault();
            if (rayon != null)
            {
                context.Rayoni.DeleteOnSubmit(rayon);
                context.Rayoni.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Ulizu(string IdDeleteUliza)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var uliza = context.Ulizi.Where(p => p.Id_Ulizi == Convert.ToInt32(IdDeleteUliza)).FirstOrDefault();
            if (uliza != null)
            {
                context.Ulizi.DeleteOnSubmit(uliza);
                context.Ulizi.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Adres(string IdDeleteAdresa)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var adres = context.Adresa.Where(p => p.Id_Adresa == Convert.ToInt32(IdDeleteAdresa)).FirstOrDefault();
            if (adres != null)
            {
                context.Adresa.DeleteOnSubmit(adres);
                context.Adresa.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Klienta(string IdDeleteKlienta)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var klient = context.Klienti.Where(p => p.Id_Klienta == Convert.ToInt32(IdDeleteKlienta)).FirstOrDefault();
            if (klient != null)
            {
                context.Klienti.DeleteOnSubmit(klient);
                context.Klienti.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Schet_Klienta(string IdDeleteSchet_Klienta)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var schet_klienta = context.Scheta.Where(p => p.Id_Scheta == Convert.ToInt32(IdDeleteSchet_Klienta)).FirstOrDefault();
            if (schet_klienta != null)
            {
                context.Scheta.DeleteOnSubmit(schet_klienta);
                context.Scheta.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Doljnost(string IdDeleteDoljnost)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var doljnost = context.Doljnosti.Where(p => p.Id_Doljnosti == Convert.ToInt32(IdDeleteDoljnost)).FirstOrDefault();
            if (doljnost != null)
            {
                context.Doljnosti.DeleteOnSubmit(doljnost);
                context.Doljnosti.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Sotrudnika(string IdDeleteSotrudnika)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var sotrudnik = context.Sotrudniki.Where(p => p.Id_Sotrudnika == Convert.ToInt32(IdDeleteSotrudnika)).FirstOrDefault();
            if (sotrudnik != null)
            {
                context.Sotrudniki.DeleteOnSubmit(sotrudnik);
                context.Sotrudniki.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Doljnost_Sotrudnik(string IdDeleteDoljnost_Sotrudnik)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var doljnost_sotrudnik = context.Sotrudniki_Doljnosti.Where(p => p.Id_Sotrudniki_Doljnosti == Convert.ToInt32(IdDeleteDoljnost_Sotrudnik)).FirstOrDefault();
            if (doljnost_sotrudnik != null)
            {
                context.Sotrudniki_Doljnosti.DeleteOnSubmit(doljnost_sotrudnik);
                context.Sotrudniki_Doljnosti.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Uslugu(string IdDeleteUslugi)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var usluga = context.Uslugi.Where(p => p.Id_Uslugi == Convert.ToInt32(IdDeleteUslugi)).FirstOrDefault();
            if (usluga != null)
            {
                context.Uslugi.DeleteOnSubmit(usluga);
                context.Uslugi.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_Klient_Usluga(string IdDeleteKlient_Usluga)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var klient_usluga = context.Klient_Uslugi.Where(p => p.Id_Klient_Uslugi == Convert.ToInt32(IdDeleteKlient_Usluga)).FirstOrDefault();
            if (klient_usluga != null)
            {
                context.Klient_Uslugi.DeleteOnSubmit(klient_usluga);
                context.Klient_Uslugi.Context.SubmitChanges();
            }
        }

        [WebMethod]
        public void Delete_IstoriyaOplati(string IdDeleteIstoriyaOplati)
        {
            Uchet_Internet_Uslug_dbDataContext context = new Uchet_Internet_Uslug_dbDataContext();
            var istoriya_oplati = context.Istoriya_Oplati.Where(p => p.Id_Istorii_oplati == Convert.ToInt32(IdDeleteIstoriyaOplati)).FirstOrDefault();
            if (istoriya_oplati != null)
            {
                context.Istoriya_Oplati.DeleteOnSubmit(istoriya_oplati);
                context.Istoriya_Oplati.Context.SubmitChanges();
            }
        }
    }
}
