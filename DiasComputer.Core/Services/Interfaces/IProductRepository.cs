using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Account;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.DTOs.Product;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Products;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface IProductRepository
    {
        public IEnumerable<AmazingProductsSliderViewModel> GetLatestAmazingProducts();
        public List<LatestProductsSliderViewModel> GetLatestProducts(int groupId);

        #region UserWishListProducts

        public IEnumerable<LatestWishListViewModel> GetLatestWishLists(int userId);
        public IEnumerable<LatestWishListViewModel> GetAllWishLists(int userId);

        #endregion

        #region Products

        SingleProductViewModel? GetProductDetails(int productId);
        List<ShortFeaturesViewModel> GetShortFeature(int productId);
        List<Feature> GetProductFeatures(int productId);
        List<Gallery> GetProductGalleries(int productId);
        Tuple<List<SingleProductInListViewModel>, int, int> GetProductForList(int groupId = 0, string searchPhrase = ""
            , int amazingProducts = 0, int pageId = 1, int minPrice = 0
            , int maxPrice = 0, int take = 0, int getType = 1, string orderByType = "newest");

        #endregion

        #region Search

        Tuple<List<SingleProductInListViewModel>, int, int> SearchProducts(string searchPhrase, string orderByType = "newest", int pageId = 1, int take = 8);

        #endregion

        #region Comments

        Tuple<List<Review>, int> GetProductComments(int productId, int pageId = 1);
        bool AddComment(Review review);
        bool ReportComment(int reviewId);

        #endregion

        #region Admin

        #region Product

        Tuple<List<ProductsForAdminViewModel>, int> GetProductsForAdmin(string? filter, bool? isAvailable, bool? isActive, bool? isOnSale, int pageId);
        ProductsForAdminViewModel GetProductDetailsForAdmin(int productId);
        bool AddNewProduct(ProductsForAdminViewModel product);
        bool UpdateProduct(ProductsForAdminViewModel product);
        Product GetProductByProductId(int productId);
        bool RemoveProduct(int productId);
        int GetMonthSoldProductCount();
        int GetAllSoldProductsCount();

        #endregion

        #region ProductFeatures

        Tuple<List<ProductFeaturesViewModel>, int> GetProductFeatures(int productId, int pageId);
        bool AddNewFeature(ProductFeaturesViewModel feature);
        ProductFeaturesViewModel GetFeatureByFeatureId(int featureId);
        bool UpdateFeature(ProductFeaturesViewModel feature);
        bool DeleteFeature(int featureId);
        Feature GetFeature(int featureId);

        #endregion

        #region Gallery

        List<Gallery> GetGalleryByProductId(int productId);
        bool AddImageToGallery(GalleriesViewModel gallery);
        bool UpdateImageGallery(GalleriesViewModel gallery);
        bool DeleteImageGallery(int galleryId);
        GalleriesViewModel GetGalleryViewModel(int galleryId);
        Gallery GetGalleryByGalleryId(int galleryId);
        string GetGalleryImageByGalleryId(int galleryId);

        #endregion

        #region Comments

        Tuple<List<AdminReviewsViewModel>, int> GetAwaitingReviews(int pageId);
        bool ReplyComment(AdminReviewsViewModel reply);
        bool SwitchCommentStatus(int reviewId);
        bool RemoveComment(int reviewId);
        Review GetReviewByReviewId(int reviewId);
        Tuple<List<AdminReviewsViewModel>, int> GetReportedComments(int pageId);
        bool UnReportComment(int reviewId);
        int GetThisMonthReviewsCount();
        int GetThisMonthReferProductRequestCount();
        int GetRecentCommentsCount();

        #endregion

        #endregion
    }
}
