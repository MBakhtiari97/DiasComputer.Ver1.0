using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [Display(Name = "نام")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string TicketSenderName { get; set; }
        [Display(Name = "آدرس ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [EmailAddress(ErrorMessage = "ایمیل وار دشده معتبر نمی باشد")]
        public string TicketSenderEmail { get; set; }
        [Display(Name = "شماره تلفن همراه")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? TicketSenderPhone { get; set; }
        [Display(Name = "موضوع تیکت")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string TicketSubject { get; set; }
        [Display(Name = "شرح تیکت")]
        [MaxLength(1200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string TicketDescription { get; set; }
        [Display(Name = "آدرس آیپی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? TicketSenderIP { get; set; }
        [Display(Name = "خوانده شده")]
        public bool IsTicketRead { get; set; }
        [Display(Name = "رسیدگی شده")]
        public bool IsTicketAnswered { get; set; }
        public bool IsDelete { get; set; }
    }
}
