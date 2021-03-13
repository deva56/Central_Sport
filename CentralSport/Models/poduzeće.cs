namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("poduzeće")]
    public partial class poduzeće
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public poduzeće()
        {
            images = new HashSet<images>();
        }

        [Key]
        [StringLength(100)]
        public string idPoduzeće { get; set; }

        [StringLength(100)]
        public string imePoduzeće { get; set; }

        [StringLength(5000)]
        public string opisPoduzeće { get; set; }

        [StringLength(200)]
        public string kontaktTelefon { get; set; }

        [StringLength(200)]
        public string kontaktEmail { get; set; }

        [StringLength(100)]
        public string primarnaDjelatnostPoduzeće { get; set; }

        [StringLength(100)]
        public string gradPoduzeće { get; set; }

        [StringLength(100)]
        public string županijaPoduzeće { get; set; }

        [StringLength(200)]
        public string ulicaPoduzeće { get; set; }

        [StringLength(100)]
        public string korisničkoImePoduzeće { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<images> images { get; set; }

        public IEnumerable<SelectListItem> Gradovi { get; set; }
        public IEnumerable<SelectListItem> Županije { get; set; }
        public IEnumerable<SelectListItem> PrimarneDjelatnosti { get; set; }
    }
}
