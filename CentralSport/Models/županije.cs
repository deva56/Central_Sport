namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("županije")]
    public partial class županije
    {
        [Key]
        [StringLength(100)]
        public string idŽupanija { get; set; }

        [Required]
        [StringLength(100)]
        public string imeŽupanije { get; set; }
    }
}
