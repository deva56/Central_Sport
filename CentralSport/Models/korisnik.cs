namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("korisnik")]
    public partial class korisnik
    {
        [Key]
        [StringLength(100)]
        public string idKorisnik { get; set; }

        [StringLength(100)]
        public string imeKorisnik { get; set; }

        [StringLength(100)]
        public string prezimeKorisnik { get; set; }

        [StringLength(5000)]
        public string kratkiOpisKorisnik { get; set; }

        [StringLength(100)]
        public string registracijskiEmailKorisnik { get; set; }

        [StringLength(100)]
        public string kontaktEmailKorisnik { get; set; }

        [StringLength(100)]
        public string kontaktTelefonKorisnik { get; set; }

        [StringLength(100)]
        public string gradKorisnik { get; set; }

        [StringLength(100)]
        public string županijaKorisnik { get; set; }

        [StringLength(200)]
        public string ulicaKorisnik { get; set; }

        public bool kontaktEmailIstiKaoRegistracijskiEmail { get; set; }

        [StringLength(200)]
        public string putanjaDoProfilneSlike { get; set; }

        [StringLength(100)]
        public string korisničkoImeKorisnik { get; set; }

        public IEnumerable<SelectListItem> Gradovi { get; set; }
        public IEnumerable<SelectListItem> Županije { get; set; }
    }
}
