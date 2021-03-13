namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("gradovi")]
    public partial class gradovi
    {
        [Key]
        [StringLength(100)]
        public string idGradovi { get; set; }

        [Required]
        [StringLength(100)]
        public string imeGradovi { get; set; }
    }
}
