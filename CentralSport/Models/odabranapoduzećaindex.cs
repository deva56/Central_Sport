namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("odabranapoduzećaindex")]
    public partial class odabranapoduzećaindex
    {
        [Key]
        [StringLength(100)]
        public string idOdabranaPoduzećaIndex { get; set; }

        [Required]
        [StringLength(200)]
        public string imePoduzeća { get; set; }

        [StringLength(200)]
        public string putanjaDoSlike { get; set; }

        [Required]
        [StringLength(1000)]
        public string opisPoduzeća { get; set; }
    }
}
