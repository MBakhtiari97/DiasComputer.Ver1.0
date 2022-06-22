using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Groups;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Permissions;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DiasComputer.DataLayer.Context
{
    public class DiasComputerContext : DbContext
    {
        public DiasComputerContext(DbContextOptions<DiasComputerContext> options) : base(options)
        {

        }

        #region DB_Set's

        #region Permissions

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region User's
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<PostDetail> PostDetails { get; set; }
        #endregion

        #region Product's

        public DbSet<Product> Products { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Review> Reviews { get; set; }

        #endregion

        #region Group's

        public DbSet<Group> Groups { get; set; }
        //public DbSet<ProductToGroup> ProductToGroups { get; set; }

        #endregion

        #region SiteResource's

        public DbSet<SiteContactDetail> SiteContactDetails { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<EmailHistory> Emails { get; set; }
        public DbSet<EmailType> EmailTypes { get; set; }

        #endregion

        #region Order's

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<TransactionToType> TransactionToTypes { get; set; }
        public DbSet<ReferOrder> ReferOrders { get; set; }

        #endregion


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region BindingData's

            #region Order

            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType() { TypeId = 1, TypeTitle = "واریز" },
                new TransactionType() { TypeId = 2, TypeTitle = "برداشت" }
                );

            #endregion

            #region User

            modelBuilder.Entity<Role>().HasData(
                new Role() { RoleId = 1, RoleName = "کاربر" },
                new Role() { RoleId = 2, RoleName = "فروشنده" },
                new Role() { RoleId = 3, RoleName = "نویسنده" },
                new Role() { RoleId = 4, RoleName = "مدیر" }
                );

            #endregion

            #region SiteResource

            modelBuilder.Entity<SiteContactDetail>().HasData(
                new SiteContactDetail() { SCD_Id = 1, SCD_TypeTitle = "شماره تماس", SCD_Value = "09121075852" },
                new SiteContactDetail() { SCD_Id = 2, SCD_TypeTitle = "شماره تماس", SCD_Value = "0218574123" },
                new SiteContactDetail() { SCD_Id = 4, SCD_TypeTitle = "آدرس ایمیل", SCD_Value = "Info@DiasComputer.com" },
                new SiteContactDetail() { SCD_Id = 5, SCD_TypeTitle = "آدرس پستی", SCD_Value = "تهران ، خیابان ولیعصر ، مجتمع نور تهران , طبقه دوم ، پلاک 131" }
            );

            modelBuilder.Entity<EmailType>().HasData(
                new EmailType() { EmailTypeId = 1, TypeTitle = "عمومی" },
                new EmailType() { EmailTypeId = 2, TypeTitle = "خصوصی" },
                new EmailType() { EmailTypeId = 3, TypeTitle = "مدیران" });

            #endregion

            #endregion

            #region QueryFilter

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Group>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Coupon>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Product>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Review>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Role>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Order>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<EmailHistory>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Logo>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Banner>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Ticket>()
                .HasQueryFilter(u => !u.IsDelete);


            #endregion

            #region DefaultValues

            modelBuilder.Entity<User>()
                .Property(u => u.BankAccountNumber)
                .HasDefaultValue("-");
            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasDefaultValue("-");
            modelBuilder.Entity<User>()
                .Property(u => u.NationalCode)
                .HasDefaultValue("-");
            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .HasDefaultValue("-");
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
