using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CentralSportV1._0._1.Models;

namespace CentralSportV1._0._1.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ModelsContext db = new ModelsContext();

        //Dohvaća sve gradove iz baze te stavlja u listu
        private IEnumerable<string> GetAllTowns()
        {
            List<gradovi> listaGradova = new List<gradovi>();
            List<string> listaGradovaString = new List<string>();

            var rez = from c in db.gradovi select c;
            listaGradova = rez.ToList();

            foreach (gradovi element in listaGradova)
            {
                listaGradovaString.Add(element.imeGradovi.ToString());
            }

            return listaGradovaString;
        }

        //Dohvaća sve županije iz baze te stavlja u listu
        private IEnumerable<string> GetAllŽupanije()
        {
            List<županije> listaŽupanija = new List<županije>();
            List<string> listaŽupanijaString = new List<string>();

            var rez = from c in db.županije select c;
            listaŽupanija = rez.ToList();

            foreach (županije element in listaŽupanija)
            {
                listaŽupanijaString.Add(element.imeŽupanije.ToString());
            }

            return listaŽupanijaString;
        }

        // Dohvaća sve sportove iz baze te stavlja u listu
        private IEnumerable<string> GetAllSports()
        {
            List<sportovi> listaSportova = new List<sportovi>();
            List<string> listaSportovaString = new List<string>();

            var rez = from c in db.sportovi select c;
            listaSportova = rez.ToList();

            foreach (sportovi element in listaSportova)
            {
                listaSportovaString.Add(element.imeSporta.ToString());
            }

            return listaSportovaString;
        }

        // Metoda koja svaki item iz IEnumerable liste pretvara u List item
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {            
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        // Glavna metoda koja se izvršavanjem na Index stranici inicijalizira liste gradova,
        // županija, sportova, model pretraživanja, odabrana poduzeća te kontakt formu
        public ActionResult Index()
        {
            IEnumerable<string> gradovi = GetAllTowns();
            IEnumerable<string> županije = GetAllŽupanije();
            IEnumerable<string> sportovi = GetAllSports();

            PretražiKompanijeJednostavniFilter model = new PretražiKompanijeJednostavniFilter();
            IEnumerable<odabranapoduzećaindex> odabranaPoduzeća = from c in db.odabranapoduzećaindex select c;
            SendEmail kontaktForma = new SendEmail();

            model.Gradovi = GetSelectListItems(gradovi);
            model.Županije = GetSelectListItems(županije);
            model.Sportovi = GetSelectListItems(sportovi);

            Tuple<PretražiKompanijeJednostavniFilter, IEnumerable<odabranapoduzećaindex>, SendEmail> tuple = Tuple.Create(model, odabranaPoduzeća, kontaktForma);

            if (TempData["status"] != null)
            {
                ViewBag.Message = TempData["status"].ToString();
                TempData.Remove("status");
            }

            return View(tuple);

        }

        // Metoda za slanje kontakt forme na naslovnoj stranici - 
        // uzima iz SendEmail modela korisnikovo ime, e-mail, poruku i subject 
        // te putem smtp protokola šalje poruku na naznačenu e-mail adresu
        [HttpPost]
        public ActionResult ContactUS(SendEmail model)
        {
            string adresaPosiljatelja = model.ClientEmail;
            string imePosiljatelja = model.ClientName;
            MailAddress posiljatelj = new MailAddress("centralsportpaup@hotmail.com", "Pošiljatelj");
            MailAddress primatelj = new MailAddress("centralsportpaup@hotmail.com", "Central Sport");
            MailMessage mm = new MailMessage(posiljatelj, primatelj);
            mm.Subject = model.Subject;
            mm.Body = "Adresa pošiljatelja: " + adresaPosiljatelja + Environment.NewLine + "Ime pošiljatelja: " + imePosiljatelja + Environment.NewLine + Environment.NewLine + model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("centralsportpaup@hotmail.com", "PAUP_Projekt_Central_Sport"); smtp.Credentials = nc;
            smtp.Send(mm);

            TempData["status"] = "Mail je poslan!";
    
            return RedirectToAction("Index");
        }

        public ActionResult RedirectToPretraživanje()
        {
            TempData["status"] = "Pretraživanje";
            return RedirectToAction("Index");
        }

        public ActionResult RedirectToKontakti()
        {
            TempData["status"] = "Kontakti";
            return RedirectToAction("Index");
        }
    }
}