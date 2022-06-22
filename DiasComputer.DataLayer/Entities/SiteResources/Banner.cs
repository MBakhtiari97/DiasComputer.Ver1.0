using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }
        [MaxLength(200)]
        public string? BannerSize { get; set; }
        [MaxLength(200)]
        public string? BannerPosition { get; set; }
        [MaxLength(200)]
        public string? BannerRedirectTo { get; set; }
        [Required]
        [MaxLength(200)]
        public string BannerAltName { get; set; }
        [Required]
        public string BannerImg { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
