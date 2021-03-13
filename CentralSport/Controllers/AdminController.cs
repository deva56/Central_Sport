using CentralSport;
using CentralSportV1._0._1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CentralSportV1._0._1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ModelsContext db = new ModelsContext();

        //----------------Pomoćne funkcije----------------------------
        #region
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

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
        //----------------Pomoćne funkcije----------------------------

        //----------------CRUD operacije za model Poduzeće----------------------------
        #region
        public ActionResult PoduzećeManagePanel()
        {
            return View("~/Views/Admin/Poduzeće/PoduzećeManagePanel.cshtml");
        }

        // GET: Poduzeće
        public ActionResult PoduzećeManageList()
        {
            return View("~/Views/Admin/Poduzeće/PoduzećeManageList.cshtml", db.poduzeće.ToList());
        }

        // GET: Admin/DetailsPoduzeće/5
        public ActionResult DetailsPoduzeće(string idPoduzeća)
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
            return View("~/Views/Admin/Poduzeće/DetailsPoduzeće.cshtml", tuple);
        }

        // GET: Poduzeće/Create
        public ActionResult CreatePoduzeće()
        {
            IEnumerable<string> listaGradova = GetAllTowns();
            IEnumerable<string> listaŽupanije = GetAllŽupanije();
            IEnumerable<string> listaDjelatnosti = GetAllDjelatnosti();

            AdminCreatePoduzeće modelPoduzeća = new AdminCreatePoduzeće();
            modelPoduzeća.Gradovi = GetSelectListItems(listaGradova);
            modelPoduzeća.Županije = GetSelectListItems(listaŽupanije);
            modelPoduzeća.PrimarneDjelatnosti = GetSelectListItems(listaDjelatnosti);

            return View("~/Views/Admin/Poduzeće/CreatePoduzeće.cshtml", modelPoduzeća);
        }

        // POST: Poduzeće/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePoduzeće([Bind(Include = "username,lozinka,imePoduzeće,opisPoduzeće,kontaktTelefon,kontaktEmail,primarnaDjelatnostPoduzeće,gradPoduzeće,županijaPoduzeće,ulicaPoduzeće")] AdminCreatePoduzeće AdminCreatePoduzeće)
        {
            var salt = "";
            salt = Crypto.GenerateSalt();
            var hashedEmail = Crypto.HashPassword(salt + AdminCreatePoduzeće.username);

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = AdminCreatePoduzeće.username, Email = hashedEmail + "@mail.com"};
                var result = await UserManager.CreateAsync(user, AdminCreatePoduzeće.lozinka);
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole("Poduzeće"));
                    await UserManager.AddToRoleAsync(user.Id, "Poduzeće");

                    poduzeće poduzeće = new poduzeće
                    {
                        idPoduzeće = user.Id,
                        gradPoduzeće = AdminCreatePoduzeće.gradPoduzeće,
                        imePoduzeće = AdminCreatePoduzeće.imePoduzeće,
                        kontaktEmail = AdminCreatePoduzeće.kontaktEmail,
                        kontaktTelefon = AdminCreatePoduzeće.kontaktTelefon,
                        opisPoduzeće = AdminCreatePoduzeće.opisPoduzeće,
                        primarnaDjelatnostPoduzeće = AdminCreatePoduzeće.primarnaDjelatnostPoduzeće,
                        županijaPoduzeće = AdminCreatePoduzeće.županijaPoduzeće,
                        ulicaPoduzeće = AdminCreatePoduzeće.ulicaPoduzeće,
                        korisničkoImePoduzeće = user.UserName
                    };
                    db.poduzeće.Add(poduzeće);
                    db.SaveChanges();

                    return RedirectToAction("PoduzećeManageList");
                }
                else
                {
                    var listaErrora = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        if (error == ("Name " + AdminCreatePoduzeće.username + " is already taken."))
                        {
                            listaErrora.Add("Ime " + AdminCreatePoduzeće.username + " je već zauzeto.");
                        }
                        //if (error.Substring(0, error.IndexOf(" ")) == "Email")
                        //{
                        //    listaErrora.Add("E-mail " + AdminCreatePoduzeće.registracijskiEmailKorisnik + " je već zauzet.");
                        //}
                    }
                    var rez = new IdentityResult(listaErrora);
                    AddErrors(rez);
                    return RedirectToAction("PoduzećeManageList");
                }
            }
            return RedirectToAction("CreatePoduzeće");
        }

        // GET: Poduzeće/Edit/5
        public ActionResult EditPoduzeće(string id)
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

            return View("~/Views/Admin/Poduzeće/EditPoduzeće.cshtml", modelPoduzeća);
        }

        // POST: Poduzeće/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPoduzeće([Bind(Include = "idPoduzeće,imePoduzeće,opisPoduzeće,kontaktTelefon,kontaktEmail,primarnaDjelatnostPoduzeće,gradPoduzeće,županijaPoduzeće,ulicaPoduzeće")] poduzeće poduzeće)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poduzeće).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PoduzećeManageList");
            }
            return View("~/Views/Admin/Poduzeće/EditPoduzeće.cshtml", poduzeće);
        }

        public ActionResult UpravljanjeSlikamaPoduzeće(string idPoduzeća)
        {
            if (idPoduzeća == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<images> images = from c in db.images where c.imageIdByPoduzeće.Equals(idPoduzeća) select c;

            UploadImagesModels uploadSlika = new UploadImagesModels();
            uploadSlika.idKorisnika = idPoduzeća;
            Tuple<IEnumerable<images>, UploadImagesModels> tuple = Tuple.Create(images, uploadSlika);

            if (TempData["shortMessage"] != null)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
                TempData.Remove("shortMessage");
            }

            return View("~/Views/Admin/Poduzeće/UpravljanjeSlikamaPoduzeće.cshtml", tuple);
        }

        // POST: Uploadaj slike
        [HttpPost]
        public ActionResult UpravljanjeSlikamaPoduzeće(UploadImagesModels uploadImagesModels)
        {
            if (uploadImagesModels.ImageFile[0] == null)
            {
                TempData["shortMessage"] = "Niste odabrali niti jednu sliku...";
                return RedirectToAction("UpravljanjeSlikamaPoduzeće", new { idPoduzeća = uploadImagesModels.idKorisnika });
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
                    return RedirectToAction("UpravljanjeSlikamaPoduzeće", new { idPoduzeća = uploadImagesModels.idKorisnika });
                }
                else
                {
                    foreach (HttpPostedFileBase file in uploadImagesModels.ImageFile)
                    {
                        string folderPath = "~/Images/";
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        images images = new images { imagePathOnDisk = folderPath + fileName, imageId = DateTime.Now.ToString("yymmssfff") + rnd.Next(1, 100000000).ToString(), imageIdByPoduzeće = uploadImagesModels.idKorisnika };
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(fileName);
                        db.images.Add(images);
                        db.SaveChanges();
                    }
                    TempData["shortMessage"] = "Slike uspješno uploadane.";
                    return RedirectToAction("UpravljanjeSlikamaPoduzeće", new { idPoduzeća = uploadImagesModels.idKorisnika });
                }
            }
        }

        public ActionResult DeletePicturePoduzeće(string idSlike, string id)
        {
            images slika = db.images.Find(idSlike);
            
            string putanjaSlike = Server.MapPath(slika.imagePathOnDisk);

            if (System.IO.File.Exists(putanjaSlike))
            {
                System.IO.File.Delete(putanjaSlike);
            }

            db.images.Remove(slika);
            db.SaveChanges();

            return RedirectToAction("UpravljanjeSlikamaPoduzeće", new { idPoduzeća = id });
        }

        // GET: Poduzeće/Delete/5
        public ActionResult DeletePoduzeće(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            poduzeće poduzeće = db.poduzeće.Find(id);
            if (poduzeće == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/Admin/Poduzeće/DeletePoduzeće.cshtml", poduzeće);
        }

        // POST: Poduzeće/Delete/5
        [HttpPost, ActionName("DeletePoduzeće")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPoduzeće(string id)
        {
            var context = new ApplicationDbContext();
            var korisnikIzBaze = context.Users.Find(id);
            context.Users.Remove(korisnikIzBaze);
            context.SaveChanges();

            poduzeće poduzeće = db.poduzeće.Find(id);
            db.poduzeće.Remove(poduzeće);
            db.SaveChanges();
            return RedirectToAction("PoduzećeManageList");
        }
        #endregion
        //----------------CRUD operacije za model Poduzeće----------------------------

        //----------------CRUD operacije za model Korisnik----------------------------
        #region
        // GET: Korisnik
        public ActionResult KorisnikManageList()
        {
            return View("~/Views/Admin/Korisnik/KorisnikManageList.cshtml", db.korisnik.ToList());
        }

        public ActionResult KorisnikManagePanel()
        {
            return View("~/Views/Admin/Korisnik/KorisnikManagePanel.cshtml");
        }

        // GET: Admin/DetailsKorisnik/5
        public ActionResult DetailsKorisnik(string idKorisnika)
        {
            if (idKorisnika == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            korisnik modelKorisnika = db.korisnik.Find(idKorisnika);
            if (modelKorisnika == null)
            {
                return HttpNotFound();
            }


            var context = new ApplicationDbContext();

            var korisnikIzBaze = context.Users.Find(idKorisnika);
            Tuple<korisnik, ApplicationUser> tuple = Tuple.Create(modelKorisnika, korisnikIzBaze);

            return View("~/Views/Admin/Korisnik/DetailsKorisnik.cshtml", tuple);
        }

        // GET: Korisnik/Create
        public ActionResult CreateKorisnik()
        {
            IEnumerable<string> listaGradova = GetAllTowns();
            IEnumerable<string> listaŽupanije = GetAllŽupanije();

            AdminCreateKorisnik modelKorisnika = new AdminCreateKorisnik();
            modelKorisnika.Gradovi = GetSelectListItems(listaGradova);
            modelKorisnika.Županije = GetSelectListItems(listaŽupanije);

            return View("~/Views/Admin/Korisnik/CreateKorisnik.cshtml", modelKorisnika);
        }

        // POST: Korisnik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateKorisnik([Bind(Include = "lozinka,username,imeKorisnik,prezimeKorisnik,lokacijaKorisnik,interesiKorisnik,kratkiOpisKorisnik,registracijskiEmailKorisnik,kontaktEmailKorisnik,kontaktTelefonKorisnik")] AdminCreateKorisnik AdminCreateKorisnik)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = AdminCreateKorisnik.username, Email = AdminCreateKorisnik.registracijskiEmailKorisnik };
                var result = await UserManager.CreateAsync(user, AdminCreateKorisnik.lozinka);
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole("Korisnik"));
                    await UserManager.AddToRoleAsync(user.Id, "Korisnik");

                    korisnik korisnik = new korisnik
                    {
                        idKorisnik = user.Id,
                        registracijskiEmailKorisnik = AdminCreateKorisnik.registracijskiEmailKorisnik,
                        gradKorisnik = AdminCreateKorisnik.gradKorisnik,
                        imeKorisnik = AdminCreateKorisnik.imeKorisnik,
                        kontaktEmailIstiKaoRegistracijskiEmail = false,
                        kontaktEmailKorisnik = AdminCreateKorisnik.kontaktEmailKorisnik,
                        kontaktTelefonKorisnik = AdminCreateKorisnik.kontaktTelefonKorisnik,
                        kratkiOpisKorisnik = AdminCreateKorisnik.kratkiOpisKorisnik,
                        prezimeKorisnik = AdminCreateKorisnik.prezimeKorisnik,
                        ulicaKorisnik = AdminCreateKorisnik.ulicaKorisnik,
                        županijaKorisnik = AdminCreateKorisnik.županijaKorisnik,
                        putanjaDoProfilneSlike = null,
                        korisničkoImeKorisnik = user.UserName
                    };
                    db.korisnik.Add(korisnik);
                    db.SaveChanges();

                    return RedirectToAction("KorisnikManageList");
                }
                else
                {
                    var listaErrora = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        if (error == ("Name " + AdminCreateKorisnik.username + " is already taken."))
                        {
                            listaErrora.Add("Ime " + AdminCreateKorisnik.username + " je već zauzeto.");
                        }
                        if (error.Substring(0, error.IndexOf(" ")) == "Email")
                        {
                            listaErrora.Add("E-mail " + AdminCreateKorisnik.registracijskiEmailKorisnik + " je već zauzet.");
                        }
                    }
                    var rez = new IdentityResult(listaErrora);
                    AddErrors(rez);
                }
            }
            return RedirectToAction("CreateKorisnik"); 
        }

        // GET: Korisnik/Edit/5
        public ActionResult EditKorisnik(string id)
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
            
            UploadImagesModels uploadSlika = new UploadImagesModels();
            uploadSlika.idKorisnika = modelKorisnika.idKorisnik;
            Tuple<korisnik, UploadImagesModels> tuple = Tuple.Create(modelKorisnika, uploadSlika);

            if (TempData["shortMessage"] != null)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
                TempData.Remove("shortMessage");
            }

            return View("~/Views/Admin/Korisnik/EditKorisnik.cshtml", tuple);
        }

        // POST: Korisnik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKorisnik([Bind(Include = "idKorisnik,imeKorisnik,prezimeKorisnik,kratkiOpisKorisnik,registracijskiEmailKorisnik,kontaktEmailKorisnik,kontaktTelefonKorisnik,gradKorisnik,županijaKorisnik,ulicaKorisnik,kontaktEmailIstiKaoRegistracijskiEmail,putanjaDoProfilneSlike")] korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                if (korisnik.kontaktEmailIstiKaoRegistracijskiEmail == true)
                {
                    korisnik.kontaktEmailKorisnik = korisnik.registracijskiEmailKorisnik;
                }

                db.Entry(korisnik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("KorisnikManageList");
            }
            return View("~/Views/Admin/Korisnik/EditKorisnik.cshtml", korisnik);
        }

        // POST: Uploadaj slike
        //[HttpPost]
        public ActionResult UpravljanjeSlikomProfilaKorisnik(UploadImagesModels uploadImagesModels)
        {
            var idKorisnika = uploadImagesModels.idKorisnika;
            if (uploadImagesModels.ImageFile[0] == null)
            {
                TempData["shortMessage"] = "Niste odabrali niti jednu sliku...";
                return RedirectToAction("EditKorisnik", new { id = idKorisnika });
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
                    return RedirectToAction("EditKorisnik", new { id = idKorisnika });
                }
                else
                {
                    foreach (HttpPostedFileBase file in uploadImagesModels.ImageFile)
                    {

                        string folderPath = "~/Images/";
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        korisnik korisnik = db.korisnik.Find(idKorisnika);
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
                    return RedirectToAction("EditKorisnik", new { id = idKorisnika });
                }
            }
        }

        // GET: Korisnik/Delete/5
        public ActionResult DeleteKorisnik(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            korisnik korisnik = db.korisnik.Find(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Korisnik/DeleteKorisnik.cshtml", korisnik);
        }

        // POST: Korisnik/Delete/5
        [HttpPost, ActionName("DeleteKorisnik")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedKorisnik(string id)
        {
            var context = new ApplicationDbContext();
            var korisnikIzBaze = context.Users.Find(id);
            context.Users.Remove(korisnikIzBaze);
            context.SaveChanges();

            korisnik korisnik = db.korisnik.Find(id);
            db.korisnik.Remove(korisnik);
            db.SaveChanges();

            return RedirectToAction("KorisnikManageList");
        }
        #endregion
        //----------------CRUD operacije za model Korisnik----------------------------

        //----------------CRUD operacije za model Županija----------------------------
        #region
        public ActionResult ŽupanijaManagePanel()
        {
            return View("~/Views/Admin/Županija/ŽupanijaManagePanel.cshtml");
        }

        public ActionResult ŽupanijaManageList()
        {
            return View("~/Views/Admin/Županija/ŽupanijaManageList.cshtml", db.županije.ToList());
        }

        // GET: Korisnik/Create
        public ActionResult CreateŽupanija()
        {
            return View("~/Views/Admin/Županija/CreateŽupanija.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateŽupanija([Bind(Include = "imeŽupanije")] AdminCreateŽupanija AdminCreateŽupanija)
        {
            Guid guid;
            guid = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                županije županije = new županije { idŽupanija = guid.ToString(), imeŽupanije = AdminCreateŽupanija.imeŽupanije };
                db.županije.Add(županije);
                db.SaveChanges();
                return RedirectToAction("ŽupanijaManageList");
            }

            return View("~/Views/Admin/Županija/CreateŽupanija.cshtml", AdminCreateŽupanija);
        }

        // GET: Korisnik/Edit/5
        public ActionResult EditŽupanija(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            županije županije = db.županije.Find(id);
            if (županije == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Županija/EditŽupanija.cshtml", županije);
        }

        // POST: Korisnik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditŽupanija([Bind(Include = "idŽupanija, imeŽupanije")] županije županije)
        {
            if (ModelState.IsValid)
            {
                db.Entry(županije).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ŽupanijaManageList");
            }
            return View("~/Views/Admin/Županija/EditŽupanija.cshtml", županije);
        }

        // GET: Korisnik/Delete/5
        public ActionResult DeleteŽupanija(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            županije županije = db.županije.Find(id);
            if (županije == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Županija/DeleteŽupanija.cshtml", županije);
        }

        // POST: Korisnik/Delete/5
        [HttpPost, ActionName("DeleteŽupanija")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedŽupanija(string id)
        {
            županije županije = db.županije.Find(id);
            db.županije.Remove(županije);
            db.SaveChanges();
            return RedirectToAction("ŽupanijaManageList");
        }
        #endregion
        //----------------CRUD operacije za model Županija----------------------------

        //----------------CRUD operacije za model Grad----------------------------
        #region
        public ActionResult GradManagePanel()
        {
            return View("~/Views/Admin/Gradovi/GradManagePanel.cshtml");
        }

        public ActionResult GradManageList()
        {
            return View("~/Views/Admin/Gradovi/GradManageList.cshtml", db.gradovi.ToList());
        }

        public ActionResult CreateGrad()
        {
            return View("~/Views/Admin/Gradovi/CreateGrad.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGrad([Bind(Include = "imeGradovi")] AdminCreateGrad AdminCreateGrad)
        {
            Guid guid;
            guid = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                gradovi gradovi = new gradovi { idGradovi = guid.ToString(), imeGradovi = AdminCreateGrad.imeGradovi };
                db.gradovi.Add(gradovi);
                db.SaveChanges();
                return RedirectToAction("GradManageList");
            }

            return View("~/Views/Admin/Gradovi/CreateGrad.cshtml", AdminCreateGrad);
        }

        public ActionResult EditGradovi(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gradovi gradovi = db.gradovi.Find(id);
            if (gradovi == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Gradovi/EditGradovi.cshtml", gradovi);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditGradovi([Bind(Include = "idGradovi, imeGradovi")] gradovi gradovi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gradovi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GradManageList");
            }
            return View("~/Views/Admin/Gradovi/EditGradovi.cshtml", gradovi);
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public ActionResult DeleteGradovi(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gradovi gradovi = db.gradovi.Find(id);
            if (gradovi == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Gradovi/DeleteGradovi.cshtml", gradovi);
        }

        // POST: Poduzeće/Delete/5
        [HttpPost, ActionName("DeleteGradovi")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGradoviConfirmed(string id)
        {
            gradovi gradovi = db.gradovi.Find(id);
            db.gradovi.Remove(gradovi);
            db.SaveChanges();
            return RedirectToAction("GradManageList");
        }
        #endregion
        //----------------CRUD operacije za model Grad----------------------------

        //----------------CRUD operacije za model OdabranaPoduzeća----------------------------
        #region
        public ActionResult OdabranaPoduzećaPanel()
        {
            return View("~/Views/Admin/OdabranaPoduzeća/OdabranaPoduzećaPanel.cshtml");
        }

        public ActionResult OdabranaPoduzećaManageList()
        {
            return View("~/Views/Admin/OdabranaPoduzeća/OdabranaPoduzećaManageList.cshtml", db.odabranapoduzećaindex.ToList());
        }

        public ActionResult DetailsOdabranoPoduzeće(string id)
        {
            odabranapoduzećaindex odabranapoduzećaindex = new odabranapoduzećaindex();
            odabranapoduzećaindex = db.odabranapoduzećaindex.Find(id);

            return View("~/Views/Admin/OdabranaPoduzeća/DetailsOdabranoPoduzeće.cshtml", odabranapoduzećaindex);
        }


        public ActionResult CreateOdabranoPoduzeće()
        {
            AdminCreateOdabranoPoduzeće adminCreateOdabranoPoduzeće = new AdminCreateOdabranoPoduzeće();

            return View("~/Views/Admin/OdabranaPoduzeća/CreateOdabranoPoduzeće.cshtml", adminCreateOdabranoPoduzeće);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOdabranoPoduzeće([Bind(Include = "imePoduzeća,opisPoduzeća,ImageFile")] AdminCreateOdabranoPoduzeće AdminCreateOdabranoPoduzeće)
        {
            AdminCreateOdabranoPoduzeće adminCreateOdabranoPoduzeće = new AdminCreateOdabranoPoduzeće();

            if (ModelState.IsValid)
            {
                Guid guid;
                guid = Guid.NewGuid();

                odabranapoduzećaindex modelPoduzeća = new odabranapoduzećaindex();

                HttpPostedFileBase file = AdminCreateOdabranoPoduzeće.ImageFile;

                string folderPath = "~/Images/";
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                modelPoduzeća.putanjaDoSlike = folderPath + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(fileName);

                modelPoduzeća.imePoduzeća = AdminCreateOdabranoPoduzeće.imePoduzeća;
                modelPoduzeća.opisPoduzeća = AdminCreateOdabranoPoduzeće.opisPoduzeća;
                modelPoduzeća.idOdabranaPoduzećaIndex = guid.ToString();
                
                db.odabranapoduzećaindex.Add(modelPoduzeća);
                db.SaveChanges();
                return RedirectToAction("OdabranaPoduzećaManageList");
            }

            return View("~/Views/Admin/OdabranaPoduzeća/CreateOdabranoPoduzeće.cshtml", adminCreateOdabranoPoduzeće);
        }

        public ActionResult EditOdabranoPoduzeće(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            odabranapoduzećaindex odabranapoduzećaindex = db.odabranapoduzećaindex.Find(id);
            if (odabranapoduzećaindex == null)
            {
                return HttpNotFound();
            }

            AdminEditOdabranoPoduzeće admiEditOdabranoPoduzeće = new AdminEditOdabranoPoduzeće
            {
                idOdabranaPoduzećaIndex = odabranapoduzećaindex.idOdabranaPoduzećaIndex,
                imePoduzeća = odabranapoduzećaindex.imePoduzeća,
                opisPoduzeća = odabranapoduzećaindex.opisPoduzeća,
                putanjaDoSlike = odabranapoduzećaindex.putanjaDoSlike,
                ImageFile = null
            };

            return View("~/Views/Admin/OdabranaPoduzeća/EditOdabranoPoduzeće.cshtml", admiEditOdabranoPoduzeće);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditOdabranoPoduzeće([Bind(Include = "idOdabranaPoduzećaIndex, imePoduzeća, opisPoduzeća, putanjaDoSlike")] AdminEditOdabranoPoduzeće AdminEditOdabranoPoduzeće)
        {
            if (ModelState.IsValid)
            {
                odabranapoduzećaindex odabranapoduzećaindex = db.odabranapoduzećaindex.Find(AdminEditOdabranoPoduzeće.idOdabranaPoduzećaIndex);
                odabranapoduzećaindex.imePoduzeća = AdminEditOdabranoPoduzeće.imePoduzeća;
                odabranapoduzećaindex.opisPoduzeća = AdminEditOdabranoPoduzeće.opisPoduzeća;
                db.Entry(odabranapoduzećaindex).State = EntityState.Modified;
                db.SaveChanges();
                    
                return RedirectToAction("OdabranaPoduzećaManageList");
                
            }
            else
            {
                return View("~/Views/Admin/OdabranaPoduzeća/EditOdabranoPoduzeće.cshtml", AdminEditOdabranoPoduzeće);

            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditOdabranoPoduzećeProfilnaSlika([Bind(Include = "idOdabranaPoduzećaIndex, ImageFile, putanjaDoSlike")] AdminCreateOdabranoPoduzeće AdminCreateOdabranoPoduzeće)
        {
            if (AdminCreateOdabranoPoduzeće.ImageFile == null)
            {
                TempData["shortMessage"] = "Niste odabrali niti jednu sliku...";
                return RedirectToAction("EditOdabranoPoduzeće", new { id = AdminCreateOdabranoPoduzeće.idOdabranaPoduzećaIndex });
            }
            else
            {
                Random rnd = new Random();
                int brojGreski = 0;

                HttpPostedFileBase file = AdminCreateOdabranoPoduzeće.ImageFile;
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


                if (brojGreski > 0)
                {
                    TempData["shortMessage"] = "Dopuštene su samo slike .png, .jpg ili .bmp formata i moraju biti minimalno 1080x800 te maksimalno 2160x1440 piksela...";
                    return RedirectToAction("EditOdabranoPoduzeće", new { id = AdminCreateOdabranoPoduzeće.idOdabranaPoduzećaIndex });
                }
                else
                {
                    HttpPostedFileBase imageFile = AdminCreateOdabranoPoduzeće.ImageFile;

                    string folderPath = "~/Images/";
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string imageExtension = Path.GetExtension(imageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + imageExtension;
                    odabranapoduzećaindex odabranapoduzećaindex = db.odabranapoduzećaindex.Find(AdminCreateOdabranoPoduzeće.idOdabranaPoduzećaIndex);
                    string putanjaSlike = Server.MapPath(odabranapoduzećaindex.putanjaDoSlike);
                    if (System.IO.File.Exists(putanjaSlike))
                    {
                        System.IO.File.Delete(putanjaSlike);
                    }

                    odabranapoduzećaindex.putanjaDoSlike = folderPath + fileName;

                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(fileName);
                    db.SaveChanges();
                }

                TempData["shortMessage"] = "Promjene uspješno spremljene";
                return RedirectToAction("EditOdabranoPoduzeće", new { id = AdminCreateOdabranoPoduzeće.idOdabranaPoduzećaIndex });
            }
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public ActionResult DeleteOdabranoPoduzeće(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            odabranapoduzećaindex odabranapoduzećaindex = db.odabranapoduzećaindex.Find(id);
            if (odabranapoduzećaindex == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/OdabranaPoduzeća/DeleteOdabranoPoduzeće.cshtml", odabranapoduzećaindex);
        }

        // POST: Poduzeće/Delete/5
        [HttpPost, ActionName("DeleteOdabranoPoduzeće")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedOdabranoPoduzeće(string id)
        {
            odabranapoduzećaindex odabranapoduzećaindex = db.odabranapoduzećaindex.Find(id);
            db.odabranapoduzećaindex.Remove(odabranapoduzećaindex);
            db.SaveChanges();
            return RedirectToAction("OdabranaPoduzećaManageList");
        }
        #endregion
        //----------------CRUD operacije za model OdabranaPoduzeća----------------------------

        //----------------CRUD operacije za model Sportovi----------------------------
        #region
        public ActionResult AktivnostiManagePanel()
        {
            return View("~/Views/Admin/Aktivnost/AktivnostiManagePanel.cshtml");
        }

        public ActionResult AktivnostiManageList()
        {
            return View("~/Views/Admin/Aktivnost/AktivnostiManageList.cshtml", db.sportovi.ToList());
        }

        public ActionResult CreateAktivnost()
        {
            return View("~/Views/Admin/Aktivnost/CreateAktivnost.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAktivnost([Bind(Include = "imeAktivnosti")] AdminCreateAktivnost AdminCreateAktivnost)
        {
            Guid guid;
            guid = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                sportovi sportovi = new sportovi { idSportovi = guid.ToString(), imeSporta = AdminCreateAktivnost.imeAktivnosti };

                db.sportovi.Add(sportovi);
                db.SaveChanges();
                return RedirectToAction("AktivnostiManageList");
            }

            return View("~/Views/Admin/Aktivnost/CreateAktivnost.cshtml", AdminCreateAktivnost);
        }

        public ActionResult EditAktivnost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sportovi sportovi = db.sportovi.Find(id);
            if (sportovi == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Aktivnost/EditAktivnost.cshtml", sportovi);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditAktivnost([Bind(Include = "idSportovi, imeSporta")] sportovi sportovi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sportovi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AktivnostiManageList");
            }
            return View("~/Views/Admin/Aktivnost/EditAktivnost.cshtml", sportovi);
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public ActionResult DeleteAktivnost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sportovi sportovi = db.sportovi.Find(id);
            if (sportovi == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Aktivnost/DeleteAktivnost.cshtml", sportovi);
        }

        // POST: Poduzeće/Delete/5
        [HttpPost, ActionName("DeleteAktivnost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedAktivnost(string id)
        {
            sportovi sportovi = db.sportovi.Find(id);
            db.sportovi.Remove(sportovi);
            db.SaveChanges();
            return RedirectToAction("AktivnostiManageList");
        }

        #endregion
        //----------------CRUD operacije za model Sportovi----------------------------

        //----------------Admin operacije----------------------------
        #region
        public ActionResult AdminMainManagingMenu()
        {
            return View(Url.Content("~/Views/Admin/Admin/AdminMainManagingMenu.cshtml"));
        }

        public ActionResult AdminManagePanel()
        {
            return View(Url.Content("~/Views/Admin/Admin/AdminManagePanel.cshtml"));
        }

        // GET: AdminTest
        public ActionResult AdminManageList()
        {
            return View("~/Views/Admin/Admin/AdminManageList.cshtml", db.admin.ToList());
        }

        // GET: AdminTest/Create
        public ActionResult CreateAdmin()
        {
            AdminCreateAdmin adminCreateAdmin = new AdminCreateAdmin();

            return View("~/Views/Admin/Admin/CreateAdmin.cshtml", adminCreateAdmin);
        }

        // POST: AdminTest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdmin([Bind(Include = "idAdmin,imeAdmin,lozinka")] AdminCreateAdmin admin)
        {
            Guid guid;
            guid = Guid.NewGuid();

            var user = new ApplicationUser { UserName = admin.imeAdmin, Email = guid.ToString() + "@mail.com" };
            var result = await UserManager.CreateAsync(user, admin.lozinka);
            if (result.Succeeded)

            {
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await UserManager.AddToRoleAsync(user.Id, "Admin");

                admin modelAdmina = new admin
                {
                    imeAdmin = admin.imeAdmin,
                    idAdmin = user.Id
                };

                db.admin.Add(modelAdmina);
                db.SaveChanges();

                return RedirectToAction("AdminManageList");
            }
            else
            {
                return RedirectToAction("CreateAdmin");
            }
        }

        // GET: AdminTest/Delete/5
        public ActionResult DeleteAdmin(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Admin/DeleteAdmin.cshtml", admin);
        }

        // POST: AdminTest/Delete/5
        [HttpPost, ActionName("DeleteAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedAdmin(string id)
        {
            var context = new ApplicationDbContext();
            var korisnikIzBaze = context.Users.Find(id);
            context.Users.Remove(korisnikIzBaze);
            context.SaveChanges();

            admin admin = db.admin.Find(id);
            db.admin.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("AdminManageList");
        }

        #endregion
        //----------------Admin operacije----------------------------
    }
}
