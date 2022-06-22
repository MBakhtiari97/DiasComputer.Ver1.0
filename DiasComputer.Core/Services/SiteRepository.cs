using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Generator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiasComputer.Core.Services
{
    public class SiteRepository : ISiteRepository
    {
        DiasComputerContext _context;

        public SiteRepository(DiasComputerContext context)
        {
            _context = context;
        }

        public bool AddCoupon(CouponsViewModel coupon)
        {
            try
            {
                var newCoupon = new Coupon()
                {
                    IsActive = coupon.IsActive,
                    ActiveFrom = coupon.ActiveFrom,
                    ActiveTill = coupon.ActiveTill,
                    CouponCode = coupon.CouponCode,
                    CouponPercent = coupon.CouponPercent,
                    CouponsCount = coupon.CouponCount
                };

                _context.Coupons.Add(newCoupon);
                _context.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCoupon(CouponsViewModel coupon)
        {
            try
            {
                var currentCoupon = GetCouponByCouponId(coupon.CouponId);

                currentCoupon.IsActive = coupon.IsActive;
                currentCoupon.ActiveFrom = coupon.ActiveFrom;
                currentCoupon.ActiveTill = coupon.ActiveTill;
                currentCoupon.CouponCode = coupon.CouponCode;
                currentCoupon.CouponPercent = coupon.CouponPercent;
                currentCoupon.CouponsCount = coupon.CouponCount;

                _context.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCoupon(int couponId)
        {
            try
            {
                var coupon = GetCouponByCouponId(couponId);

                coupon.IsDelete = true;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }


        }

        public Coupon GetCouponByCouponId(int couponId)
        {
            return _context.Coupons.Find(couponId);
        }

        public CouponsViewModel GetCouponViewModel(int couponId)
        {
            return _context.Coupons.Where(c => c.CouponId == couponId).Select(c => new CouponsViewModel()
            {
                ActiveTill = c.ActiveTill,
                ActiveFrom = c.ActiveFrom,
                IsActive = c.IsActive,
                CouponCode = c.CouponCode,
                CouponCount = c.CouponsCount,
                CouponId = c.CouponId,
                CouponPercent = c.CouponPercent,
            }).Single();
        }

        public List<SiteContactDetail> GetShopContactDetails()
        {
            return _context.SiteContactDetails.ToList();
        }

        public bool AddTicket(Ticket ticket)
        {
            try
            {
                _context.Tickets.Add(ticket);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTicket(Ticket ticket)
        {
            try
            {
                _context.Tickets.Update(ticket);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTicket(int ticketId)
        {
            try
            {
                var ticket = GetTicketByTicketId(ticketId);
                ticket.IsDelete = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Ticket GetTicketByTicketId(int ticketId)
        {
            return _context.Tickets.Find(ticketId);
        }

        public Tuple<List<Ticket>, int> GetAllTickets(bool isRead, bool isAnswered, int pageId = 1)
        {
            //Getting all tickets
            IQueryable<Ticket> tickets = _context.Tickets;

            switch (isRead)
            {
                case true:
                    tickets = tickets.Where(t => t.IsTicketRead);
                    break;
                case false:
                    tickets = tickets.Where(t => !t.IsTicketRead);
                    break;
            }

            switch (isAnswered)
            {
                case true:
                    tickets = tickets.Where(t => t.IsTicketAnswered);
                    break;
                case false:
                    tickets = tickets.Where(t => !t.IsTicketAnswered);
                    break;
            }


            //Pagination
            int take = 4;
            int skip = (pageId - 1) * take;
            int pageCount = tickets.Count() / take;

            return Tuple.Create(tickets.Skip(skip).Take(take).ToList(), pageCount);
        }

        public List<Ticket> GetAllTickets()
        {
            return _context.Tickets.ToList();
        }

        public bool AddNewDetail(SiteContactDetail detail)
        {
            try
            {
                _context.SiteContactDetails.Add(detail);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateDetail(SiteContactDetail detail)
        {
            try
            {
                _context.SiteContactDetails.Update(detail);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteDetail(int SCD_Id)
        {
            try
            {
                var contactDetails = GetDetailBySiteContactDetailId(SCD_Id);
                _context.SiteContactDetails.Remove(contactDetails);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public SiteContactDetail GetDetailBySiteContactDetailId(int SCD_Id)
        {
            return _context.SiteContactDetails.Find(SCD_Id);
        }

        public bool JoinToNewsletter(string emailAddress)
        {
            try
            {
                //If the email address wasn't exists in database , it will be create a new item
                if (!CheckEmailExistsInNewsletter(emailAddress))
                {
                    _context.NewsLetters.Add(new NewsLetter()
                    {
                        EmailAddress = emailAddress,
                        IsCanceled = false
                    });
                    _context.SaveChanges();
                    return true;
                }

                //Getting the newsletter and checking if the isCanceled was true then putting it on false and upside down
                var newsletter = GetNewsletterByEmailAddress(emailAddress);
                if (newsletter.IsCanceled == true)
                {
                    newsletter.IsCanceled = false;
                }
                else
                {
                    newsletter.IsCanceled = true;
                }

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Tuple<List<EmailHistory>, int> GetEmailHistory(int? typeId, string? filter, int pageId = 1)
        {
            //Getting emails
            IQueryable<EmailHistory> emails = _context.Emails;

            if (typeId != null)
            {
                emails = emails.Where(e => e.EmailTypeId == typeId);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                emails = emails.Where(e => e.Subject.Contains(filter));
            }

            //Pagination
            int take = 12;
            int skip = (pageId - 1) * take;
            int pageCount = emails.Count() / take;

            return Tuple.Create(emails.Skip(skip).Take(take).ToList(), pageCount);
        }

        public bool DeleteEmail(int emailId)
        {
            try
            {
                var email = GetEmailByEmailId(emailId);
                email.IsDelete = true;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public EmailHistory GetEmailByEmailId(int emailId)
        {
            return _context.Emails.Find(emailId);
        }

        public bool CheckEmailExistsInNewsletter(string emailAddress)
        {
            if (_context.NewsLetters.Any(n => n.EmailAddress == emailAddress))
                return true;

            return false;
        }

        public NewsLetter GetNewsletterByNewsletterId(int newsletterId)
        {
            return _context.NewsLetters.Find(newsletterId);
        }

        public NewsLetter GetNewsletterByEmailAddress(string emailAddress)
        {
            return _context.NewsLetters
                .Single(n => n.EmailAddress == emailAddress);
        }

        public List<Logo> GetAllBrands()
        {
            return _context.Logos.ToList();
        }

        public List<Logo> GetBrands()
        {
            return _context.Logos.Where(l => l.IsActive).ToList();
        }

        public BrandsViewModel GetBrand(int logoId)
        {
            return _context.Logos.Where(l => l.LogoId == logoId).Select(l => new BrandsViewModel()
            {
                IsActive = l.IsActive,
                LogoImg = l.LogoImg,
                RedirectTo = l.RedirectTo,
                LogoTitle = l.LogoTitle,
                LogoId = l.LogoId
            }).Single();
        }

        public bool AddNewBrand(BrandsViewModel brand)
        {
            try
            {
                var newBrand = new Logo()
                {
                    IsActive = brand.IsActive,
                    LogoImg = brand.LogoImg,
                    RedirectTo = brand.RedirectTo,
                    LogoTitle = brand.LogoTitle
                };
                _context.Logos.Add(newBrand);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateBrand(BrandsViewModel brand)
        {
            try
            {
                var currentbrand = GetBrandByBrandId(brand.LogoId);

                currentbrand.IsActive = brand.IsActive;
                currentbrand.LogoImg = brand.LogoImg;
                currentbrand.RedirectTo = brand.RedirectTo;
                currentbrand.LogoTitle = brand.LogoTitle;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteBrand(int brandId)
        {
            try
            {
                var brand = GetBrandByBrandId(brandId);
                brand.IsDelete = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Logo GetBrandByBrandId(int logoId)
        {
            return _context.Logos.Find(logoId);
        }

        public Tuple<List<CouponsViewModel>, int> GetAllCoupons(DateTime? startDate, DateTime? endDate,
            string searchPhrase = "", int pageId = 1)
        {
            //Getting all coupons
            var coupons = _context.Coupons.Select(c => new CouponsViewModel()
            {
                IsActive = c.IsActive,
                ActiveFrom = c.ActiveFrom,
                ActiveTill = c.ActiveTill,
                CouponCode = c.CouponCode,
                CouponCount = c.CouponsCount,
                CouponId = c.CouponId,
                CouponPercent = c.CouponPercent
            });

            //Effecting filters
            if (searchPhrase != null)
            {
                coupons = coupons.Where(c => c.CouponCode.Contains(searchPhrase));
            }

            if (startDate != null)
            {
                coupons = coupons.Where(c => c.ActiveFrom <= startDate);
            }

            if (endDate != null)
            {
                coupons = coupons.Where(c => c.ActiveTill >= endDate);
            }

            //Pagination
            int take = 8;
            int skip = (pageId - 1) * take;
            int pageCount = coupons.Count() / take;

            return Tuple.Create(coupons.Skip(skip).Take(take).ToList(), pageCount);
        }
    }
}
