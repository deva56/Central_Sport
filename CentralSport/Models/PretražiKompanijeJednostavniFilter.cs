using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CentralSportV1._0._1.Models
{
    public class PretražiKompanijeJednostavniFilter
    {
       
        [Display(Name = "Pretraživanje")]
        public string pojamPretraživanja { get; set; }

        public string sport { get; set; }
        public string grad { get; set; }
        public string županija { get; set; }

        public IEnumerable<SelectListItem> Gradovi { get; set; }
        public IEnumerable<SelectListItem> Sportovi { get; set; }
        public IEnumerable<SelectListItem> Županije { get; set; }

    }
}