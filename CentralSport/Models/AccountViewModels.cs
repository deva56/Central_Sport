using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace CentralSportV1._0._1.Models
{
    public class DeletePicturesOnKorisnikDeleteModel
    {
        public string idKorisnika { get; set; }
        public string putanjaDoProfilneSlike { get; set; }
    }

    public class AdminCreateAdmin
    {
        [Required(ErrorMessage = "Polje je obavezno...")]
        [StringLength(100)]
        public string lozinka { get; set; }

        [Required(ErrorMessage = "Polje je obavezno...")]
        [StringLength(100)]
        public string idAdmin { get; set; }

        [Required(ErrorMessage = "Polje je obavezno...")]
        [StringLength(100)]
        public string imeAdmin { get; set; }
    }

    public class AdminCreateOdabranoPoduzeće
    {
        [Required(ErrorMessage = "Ime poduzeća je obavezno polje..")]
        [StringLength(200,ErrorMessage = "Maksimalna dužina imena poduzeća je 200 karaktera...")]
        public string imePoduzeća { get; set; }

        [Required(ErrorMessage = "Opis poduzeća je obavezno polje...")]
        [StringLength(1000, ErrorMessage = "Maksimalna dužina opisa poduzeća je 1000 karaktera...")]
        public string opisPoduzeća { get; set; }

        [Required(ErrorMessage = "Slika poduzeća je obavezna...")]
        public HttpPostedFileBase ImageFile { get; set; }

        [StringLength(200)]
        public string putanjaDoSlike { get; set; }

        [StringLength(100)]
        public string idOdabranaPoduzećaIndex { get; set; }
    }

    public class AdminEditOdabranoPoduzeće
    {
        [Required(ErrorMessage = "Ime poduzeća je obavezno polje..")]
        [StringLength(200, ErrorMessage = "Maksimalna dužina imena poduzeća je 200 karaktera...")]
        public string imePoduzeća { get; set; }

        [Required(ErrorMessage = "Opis poduzeća je obavezno polje...")]
        [StringLength(1000, ErrorMessage = "Maksimalna dužina opisa poduzeća je 1000 karaktera...")]
        public string opisPoduzeća { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        [StringLength(200)]
        public string putanjaDoSlike { get; set; }

        [StringLength(100)]
        public string idOdabranaPoduzećaIndex { get; set; }
    }

    public class AdminCreateGrad
    {
        [Required(ErrorMessage = "Polje - Ime grada je obavezno...") ]
        [StringLength(100,ErrorMessage = "Ime grada može imati maksimalno 100 slova...")]
        public string imeGradovi { get; set; }
    }

    public class AdminCreateŽupanija
    {
        [Required(ErrorMessage = "Polje - Ime županije je obavezno...")]
        [StringLength(100, ErrorMessage = "Ime županije može imati maksimalno 100 slova...")]
        public string imeŽupanije { get; set; }
    }

    public class AdminCreateAktivnost
    {
        [Required(ErrorMessage = "Polje - Ime aktivnosti je obavezno...")]
        [StringLength(100, ErrorMessage = "Ime aktivnosti može imati maksimalno 100 slova...")]
        public string imeAktivnosti { get; set; }
    }

    public class AdminCreatePoduzeće
    {
        [Required(ErrorMessage = "Polje - Korisničko ime je obavezno...")]
        [StringLength(45)]
        public string username { get; set; }

        [Required(ErrorMessage = "Polje - Lozinka je obavezno...")]
        [StringLength(45)]
        public string lozinka { get; set; }

        [Required(ErrorMessage = "Polje - Ime poduzeća je obavezno...")]
        [StringLength(45)]
        public string imePoduzeće { get; set; }

        [StringLength(5000)]
        public string opisPoduzeće { get; set; }

        [StringLength(200)]
        public string kontaktTelefon { get; set; }

        [StringLength(200)]
        public string kontaktEmail { get; set; }

        [StringLength(100)]
        public string primarnaDjelatnostPoduzeće { get; set; }

        [StringLength(45)]
        public string gradPoduzeće { get; set; }

        [StringLength(45)]
        public string županijaPoduzeće { get; set; }

        [StringLength(200)]
        public string ulicaPoduzeće { get; set; }

        public IEnumerable<SelectListItem> Gradovi { get; set; }
        public IEnumerable<SelectListItem> Županije { get; set; }
        public IEnumerable<SelectListItem> PrimarneDjelatnosti { get; set; }
    }

    public class AdminCreateKorisnik
    {
        [Required(ErrorMessage = "Polje - Korisničko ime je obavezno...")]
        [StringLength(45)]
        public string username { get; set; }

        [Required(ErrorMessage = "Polje - Lozinka je obavezno...")]
        [StringLength(45)]
        public string lozinka { get; set; }

        [StringLength(45)]
        public string imeKorisnik { get; set; }

        [StringLength(45)]
        public string prezimeKorisnik { get; set; }

        [StringLength(5000)]
        public string kratkiOpisKorisnik { get; set; }

        [Required(ErrorMessage = "Polje - Registracijski email  je obavezno...")]
        [StringLength(45)]
        public string registracijskiEmailKorisnik { get; set; }

        [StringLength(45)]
        public string kontaktEmailKorisnik { get; set; }

        [StringLength(45)]
        public string kontaktTelefonKorisnik { get; set; }

        [StringLength(45)]
        public string gradKorisnik { get; set; }

        [StringLength(45)]
        public string županijaKorisnik { get; set; }

        [StringLength(200)]
        public string ulicaKorisnik { get; set; }

        public IEnumerable<SelectListItem> Gradovi { get; set; }
        public IEnumerable<SelectListItem> Županije { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno!")]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Zaporka je obavezna!")]
        [DataType(DataType.Password)]
        [Display(Name = "Zaporka")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [DataType(DataType.Text)]
        [Display(Name = "Korisničko ime")]
        public string KorisničkoIme { get; set; }

        [Required(ErrorMessage = "E-mail je obavezan!")]
        [EmailAddress(ErrorMessage = "E-mail adresa nije valjana!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Zaporka je obavezna!")]
        [StringLength(100, ErrorMessage = "Zaporka mora biti bar {2} karaktera dugačka.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Zaporka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Ponovi zaporku")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Zaporka i ponovljena zaporka se ne podudaraju!")]
        public string ConfirmPassword { get; set; }
    }

    public class SendEmail
    {
        public string CompanyEmail { get; set; }
        [Display(Name = "Vaše ime")]
        [Required(ErrorMessage = "Vaše ime je obavezno polje.")]
        public string ClientName { get; set; }
        [Display(Name = "Vaš e-mail")]
        [Required(ErrorMessage = "Vaš e-mail je obavezno polje.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail nije ispravan...")]
        public string ClientEmail { get; set; }
        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject poruke je obavezno polje.")]
        public string Subject { get; set; }
        [Display(Name = "Poruka")]
        [Required(ErrorMessage = "Poruka je obavezno polje.")]
        public string Body { get; set; }
    }

    public class UploadImagesModels
    {
        public HttpPostedFileBase[] ImageFile { get; set; }

        public string idKorisnika { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
