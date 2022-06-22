using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Account;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.DTOs.Product;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Groups;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.Utility.Convertors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiasComputer.Core.Services
{
    public class ProductRepository : IProductRepository
    {
        private DiasComputerContext _context;

        public ProductRepository(DiasComputerContext context)
        {
            _context = context;
        }

        public IEnumerable<AmazingProductsSliderViewModel> GetLatestAmazingProducts()
        {
            return _context.Products
                .Where(p => p.IsOnSale == true)
                .OrderByDescending(p => p.AddedDate)
                .Select(p => new AmazingProductsSliderViewModel()
                {
                    ProductId = p.ProductId,
                    ProductImage = p.ProductImg,
                    ProductPrice = p.ProductPrice,
                    ProductSalePercent = p.SalePercent,
                    ProductTitle = p.ProductName
                })
                .Take(10)
                .ToList();
        }

        public List<LatestProductsSliderViewModel> GetLatestProducts(int groupId)
        {
            return _context.Products
                .Where(g => g.GroupId == groupId)
                .OrderByDescending(p => p.AddedDate)
                .Select(g => new LatestProductsSliderViewModel()
                {
                    ProductPrice = g.ProductPrice,
                    ProductTitle = g.ProductName,
                    ProductSalePercent = g.SalePercent,
                    ProductImage = g.ProductImg,
                    ProductId = g.ProductId,
                    IsOnSale = g.IsOnSale
                })
                .Take(8)
                .ToList();
        }

        public IEnumerable<LatestWishListViewModel> GetLatestWishLists(int userId)
        {
            return _context.WishLists.Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .Select(w => new LatestWishListViewModel()
                {
                    UserId = w.UserId,
                    AvailableStatus = w.Product.AvailableStatus,
                    ProductId = w.ProductId,
                    ProductImage = w.Product.ProductImg,
                    ProductTitle = w.Product.ProductName,
                    ProductPrice = w.Product.ProductPrice,
                    WishListId = w.WishListId
                })
                .Take(2)
                .ToList();
        }

        public IEnumerable<LatestWishListViewModel> GetAllWishLists(int userId)
        {
            return _context.WishLists.Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .Select(w => new LatestWishListViewModel()
                {
                    UserId = w.UserId,
                    AvailableStatus = w.Product.AvailableStatus,
                    ProductId = w.ProductId,
                    ProductImage = w.Product.ProductImg,
                    ProductTitle = w.Product.ProductName,
                    ProductPrice = w.Product.ProductPrice,
                    WishListId = w.WishListId
                })
                .ToList();
        }

        public SingleProductViewModel? GetProductDetails(int productId)
        {
            return _context.Products.Where(p => p.ProductId == productId)
                .Select(p => new SingleProductViewModel()
                {
                    ProductImage = p.ProductImg,
                    AvailableStatus = p.AvailableStatus,
                    Brand = p.ProductBrand,
                    Color = p.ProductColors,
                    FullDescription = p.FullDescription,
                    IsOnSale = p.IsOnSale,
                    ProductEngName = p.ProductEngName,
                    ProductId = p.ProductId,
                    ProductPrice = p.ProductPrice,
                    ProductName = p.ProductName,
                    SalePercent = p.SalePercent,
                    ShortDescription = p.ShortDescription,
                    Warranty = p.WarrantyName,
                    GroupId = p.GroupId,
                    SubGroupId = p.SubGroupId
                }).SingleOrDefault();
        }

        public List<ShortFeaturesViewModel> GetShortFeature(int productId)
        {
            return _context.Features
                .Where(f => f.ProductId == productId && f.IsShortFeature)
                .Select(f => new ShortFeaturesViewModel()
                {
                    FeatureDescription = f.FeatureDescription,
                    FeatureTitle = f.FeatureTitle
                })
                .ToList();
        }

        public List<Feature> GetProductFeatures(int productId)
        {
            return _context.Features
                .Where(f => f.ProductId == productId)
                .ToList();
        }

        public List<Gallery> GetProductGalleries(int productId)
        {
            return _context.Galleries
                .Where(g => g.ProductId == productId)
                .ToList();
        }

        public Tuple<List<SingleProductInListViewModel>, int, int> GetProductForList(int groupId = 0, string searchPhrase = ""
            , int amazingProducts = 0, int pageId = 1, int minPrice = 0
            , int maxPrice = 0, int take = 8, int getType = 1, string orderByType = "newest")
        {
            IQueryable<Product> result = _context.Products;

            switch (orderByType)
            {
                case "priceUp":
                    result = result.OrderBy(r => r.ProductPrice);
                    break;
                case "priceDown":
                    result = result.OrderByDescending(r => r.ProductPrice);
                    break;
                case "newest":
                    result = result.OrderByDescending(r => r.AddedDate);
                    break;
            }

            if (!string.IsNullOrEmpty(searchPhrase))
            {
                result = result.Where(p =>
                    p.ProductName.Contains(searchPhrase) || p.Tags.Contains(searchPhrase) ||
                    p.ShortDescription.Contains(searchPhrase));
            }

            switch (getType)
            {
                case 0:
                    break;
                case 1:
                    {
                        result = result.Where(p => p.AvailableStatus);
                        break;
                    }
            }

            if (minPrice > 0)
                result = result.Where(p => p.ProductPrice > minPrice);

            if (maxPrice > 0)
                result = result.Where(p => p.ProductPrice < maxPrice);

            if (groupId != 0)
            {
                result = result.Where(p => p.GroupId == groupId || p.SubGroupId == groupId);
            }

            if (amazingProducts == 1)
            {
                result = result.Where(r => r.IsOnSale);
            }

            var rawResult = result.Select(p => new SingleProductInListViewModel()
            {
                ProductPrice = p.ProductPrice,
                AvailableStatus = p.AvailableStatus,
                IsOnSale = p.IsOnSale,
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                SalePercent = p.SalePercent,
                ProductImage = p.ProductImg,
                TotalProductsCount = p.AvailableCount
            }).ToList();

            var resultCount = result.Count();

            //Pagination
            int skip = (pageId - 1) * take;
            int pageCount = rawResult.Count() / take;

            var query = rawResult.Skip(skip)
                .Take(take).ToList();

            return Tuple.Create(query, pageCount, resultCount);
        }

        public Tuple<List<SingleProductInListViewModel>, int, int> SearchProducts(string searchPhrase, string orderByType = "newest", int pageId = 1, int take = 8)
        {
            var result = _context.Products.Where(p => p.ProductName.Contains(searchPhrase)
                                                        || p.ProductEngName.Contains(searchPhrase)
                                                        || p.ShortDescription.Contains(searchPhrase)
                                                        || p.Tags.Contains(searchPhrase)).Distinct();

            switch (orderByType)
            {
                case "priceUp":
                    result = result.OrderBy(r => r.ProductPrice);
                    break;
                case "priceDown":
                    result = result.OrderByDescending(r => r.ProductPrice);
                    break;
                case "newest":
                    result = result.OrderByDescending(r => r.AddedDate);
                    break;
            }

            var rawResult = result.Select(p => new SingleProductInListViewModel()
            {
                ProductPrice = p.ProductPrice,
                AvailableStatus = p.AvailableStatus,
                IsOnSale = p.IsOnSale,
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                SalePercent = p.SalePercent,
                ProductImage = p.ProductImg,
                TotalProductsCount = p.AvailableCount
            }).ToList();


            var resultCount = result.Count();

            //Pagination
            int skip = (pageId - 1) * take;
            int pageCount = rawResult.Count() / take;

            var query = rawResult.Skip(skip)
                .Take(take).ToList();

            return Tuple.Create(query, pageCount, resultCount);
        }

        public Tuple<List<Review>, int> GetProductComments(int productId, int pageId = 1)
        {
            int take = 10;
            int skip = (pageId - 1) * take;
            int pageCount = _context.Reviews.Count(r => r.ProductId == productId) / take;

            var reviews = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.ReviewDate)
                .Skip(skip)
                .Take(take)
                .ToList();

            var reviewQuery = reviews.Skip(skip).Take(take).ToList();
            return Tuple.Create(reviews, pageCount);
        }

        public bool AddComment(Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool ReportComment(int reviewId)
        {
            try
            {
                var review = GetReviewByReviewId(reviewId);
                review.IsReported = true;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Tuple<List<ProductsForAdminViewModel>, int> GetProductsForAdmin(string? filter, bool? isAvailable, bool? isActive, bool? isOnSale, int pageId)
        {
            //Getting products 
            IQueryable<Product> products = _context.Products;

            //Checking if the filter is not null
            if (filter != null)
            {
                products = products.Where(p =>
                    p.ProductName.Contains(filter)
                    || p.ProductBrand.Contains(filter));
            }

            //Check for other filters 
            if (isActive == true)
            {
                products = products.Where(p => p.IsActive);
            }

            if (isOnSale == true)
            {
                products = products.Where(p => p.IsOnSale);
            }

            if (isAvailable == true)
            {
                products = products.Where(p => p.AvailableStatus);
            }

            //Pagination
            int take = 10;
            int skip = (pageId - 1) * take;
            int productsPageCount = products.Count() / take;


            //Creating final products
            var finalProducts = products
                .Include(p => p.Group)
                .Select(p => new ProductsForAdminViewModel()
                {
                    ProductPrice = p.ProductPrice,
                    GroupId = p.GroupId,
                    AddedDate = p.AddedDate,
                    AvailableCount = p.AvailableCount,
                    AvailableStatus = p.AvailableStatus,
                    FullDescription = p.FullDescription,
                    IsActive = p.IsActive,
                    IsOnSale = p.IsOnSale,
                    ProductBrand = p.ProductBrand,
                    ProductColors = p.ProductColors,
                    ProductEngName = p.ProductEngName,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    SalePercent = p.SalePercent,
                    ProductImg = p.ProductImg,
                    ShortDescription = p.ShortDescription,
                    SubGroupId = p.SubGroupId,
                    Tags = p.Tags,
                    WarrantyName = p.WarrantyName
                })
                .OrderByDescending(p => p.AddedDate)
                .Skip(skip)
                .Take(take)
                .ToList();

            return Tuple.Create(finalProducts, productsPageCount);
        }

        public ProductsForAdminViewModel GetProductDetailsForAdmin(int productId)
        {
            return _context.Products.Where(p => p.ProductId == productId).Include(p => p.Group)
                .Select(p => new ProductsForAdminViewModel()
                {
                    ProductPrice = p.ProductPrice,
                    GroupId = p.GroupId,
                    AddedDate = p.AddedDate,
                    AvailableCount = p.AvailableCount,
                    AvailableStatus = p.AvailableStatus,
                    FullDescription = p.FullDescription,
                    IsActive = p.IsActive,
                    IsOnSale = p.IsOnSale,
                    ProductBrand = p.ProductBrand,
                    ProductColors = p.ProductColors,
                    ProductEngName = p.ProductEngName,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    SalePercent = p.SalePercent,
                    ProductImg = p.ProductImg,
                    ShortDescription = p.ShortDescription,
                    SubGroupId = p.SubGroupId,
                    Tags = p.Tags,
                    WarrantyName = p.WarrantyName,
                }).Single();
        }

        public bool AddNewProduct(ProductsForAdminViewModel product)
        {
            try
            {
                Product newProduct = new Product()
                {
                    AddedDate = DateTime.Now,
                    AvailableCount = product.AvailableCount,
                    GroupId = product.GroupId,
                    ProductPrice = product.ProductPrice,
                    AvailableStatus = product.AvailableStatus,
                    FullDescription = product.FullDescription,
                    IsActive = product.IsActive,
                    IsDelete = false,
                    IsOnSale = product.IsOnSale,
                    ProductBrand = product.ProductBrand,
                    ProductColors = product.ProductColors,
                    ProductEngName = product.ProductEngName,
                    ProductName = product.ProductName,
                    ProductImg = product.ProductImg,
                    SalePercent = product.SalePercent,
                    ShortDescription = product.ShortDescription,
                    SubGroupId = product.SubGroupId,
                    Tags = product.Tags,
                    WarrantyName = product.WarrantyName
                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProduct(ProductsForAdminViewModel product)
        {
            try
            {
                var currentProduct = GetProductByProductId(product.ProductId);

                currentProduct.AvailableCount = product.AvailableCount;
                currentProduct.AvailableStatus = product.AvailableStatus;
                currentProduct.FullDescription = product.FullDescription;
                currentProduct.GroupId = product.GroupId;
                currentProduct.IsActive = product.IsActive;
                currentProduct.IsOnSale = product.IsOnSale;
                currentProduct.ProductBrand = product.ProductBrand;
                currentProduct.ProductColors = product.ProductColors;
                currentProduct.ProductEngName = product.ProductEngName;
                currentProduct.ProductImg = product.ProductImg;
                currentProduct.ProductName = product.ProductName;
                currentProduct.ProductPrice = product.ProductPrice;
                currentProduct.SalePercent = product.SalePercent;
                currentProduct.ShortDescription = product.ShortDescription;
                currentProduct.SubGroupId = product.SubGroupId;
                currentProduct.Tags = product.Tags;
                currentProduct.WarrantyName = product.WarrantyName;

                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Product GetProductByProductId(int productId)
        {
            return _context.Products
                .Find(productId);
        }

        public bool RemoveProduct(int productId)
        {
            try
            {
                var product = GetProductByProductId(productId);
                product.IsDelete = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetMonthSoldProductCount()
        {
            return _context.Orders
                .Where(o => o.IsFinally
                            && o.OrderDate >= GetCustomDates.GetMonthStartDate()
                            && o.OrderDate <= GetCustomDates.GetMonthEndDate())
                .Select(o => o.OrderDetails)
                .Count();
        }
        public int GetAllSoldProductsCount()
        {
            return _context.Orders
                .Where(o => o.IsFinally)
                .Select(o => o.OrderDetails)
                .Count();
        }

        public Tuple<List<ProductFeaturesViewModel>, int> GetProductFeatures(int productId, int pageId)
        {
            //Getting features
            var features = _context.Features
                .Where(f => f.ProductId == productId)
                .Select(f => new ProductFeaturesViewModel()
                {
                    FeatureTitle = f.FeatureTitle,
                    ParentId = f.ParentId,
                    FeatureDescription = f.FeatureDescription,
                    FeatureId = f.FeatureId,
                    IsShortFeature = f.IsShortFeature,
                    ProductId = f.ProductId
                })
                .ToList();

            //Pagination
            int take = 12;
            int skip = (pageId - 1) * take;
            int pageCount = features.Count() / take;

            //Getting required features
            features = features.Skip(skip).Take(take).ToList();

            return Tuple.Create(features, pageCount);
        }

        public bool AddNewFeature(ProductFeaturesViewModel feature)
        {
            try
            {
                Feature newFeature = new Feature()
                {
                    FeatureDescription = feature.FeatureDescription,
                    FeatureTitle = feature.FeatureTitle,
                    IsShortFeature = feature.IsShortFeature,
                    ParentId = feature.ParentId,
                    ProductId = feature.ProductId
                };
                _context.Features.Add(newFeature);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }


        }

        public ProductFeaturesViewModel GetFeatureByFeatureId(int featureId)
        {
            return _context.Features.Where(f => f.FeatureId == featureId).Select(f => new ProductFeaturesViewModel()
            {
                FeatureDescription = f.FeatureDescription,
                FeatureId = f.FeatureId,
                FeatureTitle = f.FeatureTitle,
                IsShortFeature = f.IsShortFeature,
                ParentId = f.ParentId,
                ProductId = f.ProductId
            }).Single();
        }

        public bool UpdateFeature(ProductFeaturesViewModel feature)
        {
            try
            {
                var currentFeature = GetFeature(feature.FeatureId);

                currentFeature.FeatureDescription = feature.FeatureDescription;
                currentFeature.FeatureTitle = feature.FeatureTitle;
                currentFeature.IsShortFeature = feature.IsShortFeature;
                currentFeature.ParentId = feature.ParentId;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFeature(int featureId)
        {
            try
            {
                var currentFeature = GetFeature(featureId);

                _context.Features.Remove(currentFeature);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Feature GetFeature(int featureId)
        {
            return _context.Features
                .Find(featureId);
        }

        public List<Gallery> GetGalleryByProductId(int productId)
        {
            return _context.Galleries
                .Where(g => g.ProductId == productId)
                .ToList();
        }

        public bool AddImageToGallery(GalleriesViewModel gallery)
        {
            try
            {
                Gallery newGallery = new Gallery()
                {
                    ImgName = gallery.ImgName,
                    ImgAltName = gallery.ImgAltName,
                    ProductId = gallery.ProductId
                };
                _context.Galleries.Add(newGallery);
                _context.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool UpdateImageGallery(GalleriesViewModel gallery)
        {

            try
            {
                var currentGallery = GetGalleryByGalleryId(gallery.GalleryId);

                currentGallery.ImgAltName = gallery.ImgAltName;
                currentGallery.ImgName = gallery.ImgName;

                _context.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteImageGallery(int galleryId)
        {
            try
            {
                var currentGallery = GetGalleryByGalleryId(galleryId);

                _context.Galleries.Remove(currentGallery);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public GalleriesViewModel GetGalleryViewModel(int galleryId)
        {
            return _context.Galleries
                .Where(g => g.GalleryId == galleryId)
                .Select(g => new GalleriesViewModel()
                {
                    ProductId = g.ProductId,
                    ImgName = g.ImgName,
                    ImgAltName = g.ImgAltName,
                    GalleryId = g.GalleryId
                }).Single();
        }

        public Gallery GetGalleryByGalleryId(int galleryId)
        {
            return _context.Galleries
                .Find(galleryId);
        }

        public string GetGalleryImageByGalleryId(int galleryId)
        {
            return _context.Galleries
                .Find(galleryId)
                .ImgName;
        }

        public Tuple<List<AdminReviewsViewModel>, int> GetAwaitingReviews(int pageId)
        {
            //Getting awaiting confirmation reviews
            var awaitingComments = _context.Reviews
                .Where(r => !r.IsConfirmed)
                .Include(r => r.User)
                .Include(r => r.Product)
                .Select(r => new AdminReviewsViewModel()
                {
                    UserId = r.UserId,
                    ProductId = r.ProductId,
                    UserName = r.User.UserName,
                    ParentId = r.ParentId,
                    EmailAddress = r.User.EmailAddress,
                    ReviewDate = r.ReviewDate,
                    ReviewId = r.ReviewId,
                    ReviewText = r.ReviewText,
                    ProductName = r.Product.ProductName
                })
                .ToList();

            //Getting page count
            int take = 12;
            int skip = (pageId - 1) * take;
            int pageCount = awaitingComments.Count() / take;

            //Getting required comments
            awaitingComments = awaitingComments.Skip(skip).Take(take).ToList();

            return Tuple.Create(awaitingComments, pageCount);
        }

        public bool ReplyComment(AdminReviewsViewModel reply)
        {
            try
            {
                Review newComment = new Review()
                {
                    IsConfirmed = true,
                    IsDelete = false,
                    ParentId = reply.ParentId,
                    ProductId = reply.ProductId,
                    ReviewDate = DateTime.Now,
                    ReviewText = reply.ReviewText,
                    UserId = reply.UserId
                };
                _context.Reviews.Add(newComment);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SwitchCommentStatus(int reviewId)
        {
            try
            {
                //Getting review
                var review = GetReviewByReviewId(reviewId);

                //Changing the isConfirmed Status
                if (review.IsConfirmed)
                    review.IsConfirmed = false;
                else
                    review.IsConfirmed = true;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveComment(int reviewId)
        {
            try
            {
                //Getting review
                var review = GetReviewByReviewId(reviewId);

                review.IsDelete = true;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Review GetReviewByReviewId(int reviewId)
        {
            return _context.Reviews
                .Find(reviewId);
        }

        public Tuple<List<AdminReviewsViewModel>, int> GetReportedComments(int pageId)
        {
            //Getting reported reviews
            var reportedComments = _context.Reviews
                .Where(r => r.IsReported)
                .Include(r => r.User)
                .Include(r => r.Product)
                .Select(r => new AdminReviewsViewModel()
                {
                    UserId = r.UserId,
                    ProductId = r.ProductId,
                    UserName = r.User.UserName,
                    ParentId = r.ParentId,
                    EmailAddress = r.User.EmailAddress,
                    ReviewDate = r.ReviewDate,
                    ReviewId = r.ReviewId,
                    ReviewText = r.ReviewText,
                    ProductName = r.Product.ProductName
                })
                .ToList();

            //Getting page count
            int take = 12;
            int skip = (pageId - 1) * take;
            int pageCount = reportedComments.Count() / take;

            //Getting required comments
            reportedComments = reportedComments.Skip(skip).Take(take).ToList();

            return Tuple.Create(reportedComments, pageCount);
        }

        public bool UnReportComment(int reviewId)
        {
            try
            {
                var review = GetReviewByReviewId(reviewId);
                review.IsReported = false;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetThisMonthReviewsCount()
        {
            return _context.Reviews.Count(r => r.ReviewDate >= GetCustomDates.GetMonthStartDate()
                                               && r.ReviewDate <= GetCustomDates.GetMonthEndDate());
        }

        public int GetThisMonthReferProductRequestCount()
        {
            return _context.ReferOrders.Count(ro => ro.ReferSubmitDate >= GetCustomDates.GetMonthStartDate()
                                                    && ro.ReferSubmitDate < GetCustomDates.GetMonthEndDate());
        }

        public int GetRecentCommentsCount()
        {
            return _context.Reviews.Count(r => !r.IsConfirmed);
        }
    }
}
