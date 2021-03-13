namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("images")]
    public partial class images
    {
        [Key]
        [StringLength(100)]
        public string imageId { get; set; }

        [Required]
        [StringLength(100)]
        public string imageIdByPoduzeće { get; set; }

        [Required]
        [StringLength(200)]
        public string imagePathOnDisk { get; set; }

        public virtual poduzeće poduzeće { get; set; }
    }
}
