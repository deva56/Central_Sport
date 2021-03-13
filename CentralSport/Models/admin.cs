namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("admin")]
    public partial class admin
    {
        [Key]
        [Required(ErrorMessage = "Polje je obavezno...")]
        [StringLength(100)]
        public string idAdmin { get; set; }

        [Required(ErrorMessage = "Polje je obavezno...")]
        [StringLength(100)]
        public string imeAdmin { get; set; }
    }
}
