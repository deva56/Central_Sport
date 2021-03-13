namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sportovi")]
    public partial class sportovi
    {
        [Key]
        [StringLength(100)]
        public string idSportovi { get; set; }

        [Required]
        [StringLength(100)]
        public string imeSporta { get; set; }
    }
}
