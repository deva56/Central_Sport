using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using CentralSportV1._0._1.Models;
using Microsoft.AspNet.Identity;


namespace CentralSportV1._0._1.Controllers
{
    [Authorize(Roles = "Korisnik, Poduzeće, Admin")]
    public class PoduzećeController : Controller
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
        private IEnumerable<string> GetAllDjelatnosti()
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

        [AllowAnonymous]
        public ActionResult VratiPretragu(PretražiKompanijeJednostavniFilter model)
        {
            string županija = model.županija;
            string grad = model.grad;
            string sport = model.sport;
            string IDString = model.pojamPretraživanja;

            List<poduzeće> rezList = new List<poduzeće>();

            var rezultat = from c in db.poduzeće select c;


            if (IDString == null && grad == null && sport == null && županija == null)
            {
                rezultat = from c in db.poduzeće select c;
            }
            else if (IDString != null && grad != null && sport != null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.gradPoduzeće.Equals(grad)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           && c.županijaPoduzeće.Equals(županija)
                           select c;
            }
            else if (IDString != null && grad == null && sport == null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           select c;
            }
            else if (IDString == null && grad != null && sport == null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.gradPoduzeće.Equals(grad)
                           select c;
            }
            else if (IDString == null && grad == null && sport != null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }
            else if (IDString == null && grad == null && sport == null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.županijaPoduzeće.Equals(županija)
                           select c;
            }
            else if (IDString != null && grad != null && sport == null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.gradPoduzeće.Equals(grad)
                           select c;
            }
            else if (IDString != null && grad == null && sport != null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }
            else if (IDString != null && grad == null && sport == null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.županijaPoduzeće.Equals(županija)
                           select c;
            }
            else if (IDString == null && grad != null && sport != null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.gradPoduzeće.Equals(grad)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }
            else if (IDString == null && grad != null && sport == null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.gradPoduzeće.Equals(grad)
                           && c.županijaPoduzeće.Equals(županija)
                           select c;
            }
            else if (IDString == null && grad == null && sport != null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.županijaPoduzeće.Equals(županija)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }
            else if (IDString != null && grad != null && sport != null && županija == null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.gradPoduzeće.Equals(grad)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }
            else if (IDString != null && grad == null && sport != null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.županijaPoduzeće.Equals(županija)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }
            else if (IDString != null && grad != null && sport == null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.imePoduzeće.Equals(IDString)
                           && c.gradPoduzeće.Equals(grad)
                           && c.županijaPoduzeće.Equals(županija)
                           select c;
            }
            else if (IDString == null && grad != null && sport != null && županija != null)
            {
                rezultat = from c in db.poduzeće
                           where c.županijaPoduzeće.Equals(županija)
                           && c.gradPoduzeće.Equals(grad)
                           && c.primarnaDjelatnostPoduzeće.Equals(sport)
                           select c;
            }


            if (rezultat.Count() > 0)
            {
                rezList = rezultat.ToList();
            }
            else
            {
                //Vrati korisniku poruku da nema rezultata pretrage
                return View("NemaRezultataPretrage");
            }

            return View(rezList);
        }

        [AllowAnonymous]
        public ActionResult DetaljiOPretraženojKompaniji(string idPoduzeća)
        {
            if (idPoduzeća == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poduzeće modelPoduzeća = db.poduzeće.Find(idPoduzeća);
            if (modelPoduzeća == null)
            {
                return HttpNotFound();
            }
            IEnumerable<images> modelSlika = from c in db.images where c.imageIdByPoduzeće.Equals(idPoduzeća) select c;

            Tuple<IEnumerable<images>, poduzeće> tuple = Tuple.Create(modelSlika, modelPoduzeća);
            return View(tuple);
        }

        // GET: Poduzeće/Details/5
        public ActionResult Details(string idPoduzeća)
        {
            if (idPoduzeća == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poduzeće poduzeće = db.poduzeće.Find(idPoduzeća);
            if (poduzeće == null)
            {
                return HttpNotFound();
            }

            if (idPoduzeća != User.Identity.GetUserId())
            {
                return View("DetaljiOPretraženojKompaniji", poduzeće);
            }

            return View(poduzeće);
        }

        // GET: Poduzeće/Edit/5
        public ActionResult Edit(string id)
        {
            IEnumerable<string> listaGradova = GetAllTowns();
            IEnumerable<string> listaŽupanije = GetAllŽupanije();
            IEnumerable<string> listaDjelatnosti = GetAllDjelatnosti();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            poduzeće modelPoduzeća = db.poduzeće.Find(id);
            modelPoduzeća.Gradovi = GetSelectListItems(listaGradova);
            modelPoduzeća.Županije = GetSelectListItems(listaŽupanije);
            modelPoduzeća.PrimarneDjelatnosti = GetSelectListItems(listaDjelatnosti);
            
            if (modelPoduzeća == null)
            {
                return HttpNotFound();
            }
            if (id != User.Identity.GetUserId())
            {
                return View("NeovlašteniPristup");
            }
            return View(modelPoduzeća);
        }

        // POST: Poduzeće/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPoduzeće,imePoduzeće,opisPoduzeće,kontaktTelefon,kontaktEmail,primarnaDjelatnostPoduzeće,gradPoduzeće,županijaPoduzeće,ulicaPoduzeće")] poduzeće poduzeće)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poduzeće).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { idPoduzeća = User.Identity.GetUserId() });
            }
            return View(poduzeće);
        }

        public ActionResult UpravljanjeSlikama(string idPoduzeća)
        {
            if (idPoduzeća == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<images> images = from c in db.images where c.imageIdByPoduzeće.Equals(idPoduzeća) select c;

            if (idPoduzeća != User.Identity.GetUserId())
            {
                return View("NeovlašteniPristup");
            }

            UploadImagesModels uploadSlika = new UploadImagesModels();
            Tuple<IEnumerable<images>, UploadImagesModels> tuple = Tuple.Create(images, uploadSlika);

            if (TempData["shortMessage"] != null)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
                TempData.Remove("shortMessage");
            }

            return View(tuple);
        }

        // POST: Uploadaj slike
        [HttpPost]
        public ActionResult UpravljanjeSlikama(UploadImagesModels uploadImagesModels)
        {
            if (uploadImagesModels.ImageFile[0] == null)
            {
                TempData["shortMessage"] = "Niste odabrali niti jednu sliku...";
                return RedirectToAction("UpravljanjeSlikama", new { idPoduzeća = User.Identity.GetUserId() });
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
                    return RedirectToAction("UpravljanjeSlikama", new { idPoduzeća = User.Identity.GetUserId() });
                }
                else
                {
                    foreach (HttpPostedFileBase file in uploadImagesModels.ImageFile)
                    {
                        string folderPath = "~/Images/";
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        images images = new images { imagePathOnDisk = folderPath + fileName, imageId = DateTime.Now.ToString("yymmssfff") + rnd.Next(1, 100000000).ToString(), imageIdByPoduzeće = User.Identity.GetUserId() };
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(fileName);
                        db.images.Add(images);
                        db.SaveChanges();
                    }
                    TempData["shortMessage"] = "Slike uspješno uploadane.";
                    return RedirectToAction("UpravljanjeSlikama", new { idPoduzeća = User.Identity.GetUserId() });
                }
            }
        }

        public ActionResult DeletePicture(string idSlike)
        {
            string id = User.Identity.GetUserId();
            images slika = db.images.Find(idSlike);

            if (slika.imageIdByPoduzeće != id)
            {
                return View("NeovlašteniPristup");
            }
            else
            {
                string putanjaSlike = Server.MapPath(slika.imagePathOnDisk);

                if (System.IO.File.Exists(putanjaSlike))
                {
                    System.IO.File.Delete(putanjaSlike);
                }

                db.images.Remove(slika);
                db.SaveChanges();
                return RedirectToAction("UpravljanjeSlikama", new { idPoduzeća = id });
            }
        }

        public ActionResult RedirectToDetails()
        {
            string id = User.Identity.GetUserId();
            return RedirectToAction("Details", new { idPoduzeća = id });
        }

        public ActionResult RedirectToListaSlika()
        {

            string id = User.Identity.GetUserId();
            return RedirectToAction("VratiListuSlika", new { idPoduzeća = id });
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
