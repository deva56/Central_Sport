using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CentralSportV1._0._1.Models;
using Microsoft.AspNet.Identity;

namespace CentralSportV1._0._1.Controllers
{
    [Authorize(Roles = "Korisnik, Poduzeće, Admin")]
    public class KorisnikController : Controller
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

        // GET: Korisnik/Details/5
        public ActionResult Details(string idKorisnika)
        {
            if (idKorisnika == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            korisnik korisnik = db.korisnik.Find(idKorisnika);
           
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            if (idKorisnika != User.Identity.GetUserId())
            {
                return View("NeovlašteniPristup");
            }

            return View(korisnik);
        }

        // GET: Korisnik/Edit/5
        public ActionResult Edit(string id)
        {
            IEnumerable<string> listaGradova = GetAllTowns();
            IEnumerable<string> listaŽupanije = GetAllŽupanije();
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            korisnik modelKorisnika = db.korisnik.Find(id);
            modelKorisnika.Gradovi = GetSelectListItems(listaGradova);
            modelKorisnika.Županije = GetSelectListItems(listaŽupanije);


            if (modelKorisnika == null)
            {
                return HttpNotFound();
            }
            if (id != User.Identity.GetUserId())
            {
                return View("NeovlašteniPristup");
            }
            UploadImagesModels uploadSlika = new UploadImagesModels();
            Tuple<korisnik, UploadImagesModels> tuple = Tuple.Create(modelKorisnika, uploadSlika);

            if (TempData["shortMessage"] != null)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
                TempData.Remove("shortMessage");
            }

            return View(tuple);
        }

        // POST: Korisnik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idKorisnik,imeKorisnik,prezimeKorisnik,kratkiOpisKorisnik,registracijskiEmailKorisnik,kontaktEmailKorisnik,kontaktTelefonKorisnik,gradKorisnik,županijaKorisnik,ulicaKorisnik,kontaktEmailIstiKaoRegistracijskiEmail,putanjaDoProfilneSlike")] korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                if (korisnik.kontaktEmailIstiKaoRegistracijskiEmail == true)
                {
                    korisnik.kontaktEmailKorisnik = korisnik.registracijskiEmailKorisnik;
                }
                
                db.Entry(korisnik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { idKorisnika = User.Identity.GetUserId() });
            }
            return View(korisnik);
        }

        // POST: Uploadaj slike
        [HttpPost]
        public ActionResult UpravljanjeSlikomProfila(UploadImagesModels uploadImagesModels)
        {
            string idKorisnika = User.Identity.GetUserId();

            if (uploadImagesModels.ImageFile[0] == null)
            {
                TempData["shortMessage"] = "Niste odabrali niti jednu sliku...";
                return RedirectToAction("Edit", new { id = idKorisnika });
            }
            else
            {
                Random rnd = new Random();
                int brojGreski = 0;

                foreach (HttpPostedFileBase file in uploadImagesModels.ImageFile)
                {
                    string extension = Path.GetExtension(file.FileName);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream, true, true);
                    int width = image.Width;
                    int height = image.Height;
                    if ((extension == ".png" || extension == ".jpg" || extension == ".bmp" || extension == ".jpeg") && width >= 1080 && width <= 2160 && height >= 800 && height <= 1440)
                    {
                        //
                    }
                    else
                    {
                        brojGreski++;
                    }
                }

                if (brojGreski > 0)
                {
                    TempData["shortMessage"] = "Dopuštene su samo slike .png, .jpg ili .bmp formata i moraju biti minimalno 1080x800 te maksimalno 2160x1440 piksela...";
                    return RedirectToAction("Edit", new { id = idKorisnika });
                }
                else
                {
                    foreach (HttpPostedFileBase file in uploadImagesModels.ImageFile)
                    {

                        string folderPath = "~/Images/";
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        korisnik korisnik = db.korisnik.Find(User.Identity.GetUserId());
                        string putanjaSlike = Server.MapPath(korisnik.putanjaDoProfilneSlike);
                        if (System.IO.File.Exists(putanjaSlike))
                        {
                            System.IO.File.Delete(putanjaSlike);
                        }
                        korisnik.putanjaDoProfilneSlike = folderPath + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(fileName);
                        db.SaveChanges();
                    }

                    TempData["shortMessage"] = "Slika uspješno uploadana...";
                    return RedirectToAction("Edit", new { id = idKorisnika });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
