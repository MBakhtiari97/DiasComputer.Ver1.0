using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class Slider
    {
        [Key]
        public int SlideId { get; set; }
        [MaxLength(200)]
        public string? SlideRedirectTo { get; set; }
        [Required]
        [MaxLength(200)]
        public string SlideAltName { get; set; }
        [Required]
        public string SlideImg { get; set; }
    }
}
