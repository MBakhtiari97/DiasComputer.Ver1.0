using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface ISiteRepository
    {
        #region ContactUs

        List<SiteContactDetail> GetShopContactDetails();
        bool AddNewDetail(SiteContactDetail detail);
        bool UpdateDetail(SiteContactDetail detail);
        bool DeleteDetail(int SCD_Id);
        SiteContactDetail GetDetailBySiteContactDetailId(int SCD_Id);

        #endregion

        #region Tickets

        bool AddTicket(Ticket ticket);
        bool UpdateTicket(Ticket ticket);
        bool DeleteTicket(int ticketId);
        Ticket GetTicketByTicketId(int ticketId);
        Tuple<List<Ticket>,int> GetAllTickets(bool isRead,bool isAnswered,int pageId=1);

        #endregion

        #region Emails

        Tuple<List<EmailHistory>, int> GetEmailHistory(int? typeId, string? filter, int pageId = 1);
        bool DeleteEmail(int emailId);
        EmailHistory GetEmailByEmailId(int emailId);

        #endregion

        #region Newsletter

        bool CheckEmailExistsInNewsletter(string emailAddress);
        NewsLetter GetNewsletterByNewsletterId(int newsletterId);
        NewsLetter GetNewsletterByEmailAddress(string emailAddress);
        bool JoinToNewsletter(string emailAddress);

        #endregion

        #region Brands

        List<Logo> GetAllBrands();
        List<Logo> GetBrands();
        BrandsViewModel GetBrand(int logoId);
        bool AddNewBrand(BrandsViewModel brand);
        bool UpdateBrand(BrandsViewModel brand);
        bool DeleteBrand(int brandId);
        Logo GetBrandByBrandId(int logoId);

        #endregion

        #region Coupons

        Tuple<List<CouponsViewModel>, int> GetAllCoupons(DateTime? startDate, DateTime? endDate,
            string searchPhrase = "", int pageId = 1);

        bool AddCoupon(CouponsViewModel coupon);
        bool UpdateCoupon(CouponsViewModel coupon);
        bool DeleteCoupon(int couponId);
        Coupon GetCouponByCouponId(int couponId);
        CouponsViewModel GetCouponViewModel(int couponId);

        #endregion
    }
}
