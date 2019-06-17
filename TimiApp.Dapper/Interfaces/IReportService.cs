using System.Collections.Generic;
using System.Threading.Tasks;
using TimiApp.Dapper.ViewModels;
using Utilities.DTOs;

namespace TimiApp.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetRevenueReportAsync(string fromDate, string toDate);
        Task<IEnumerable<BestSellerOfCategoryViewModel>> GetBestSellerOfCategory();
        Task<IEnumerable<MostReceivingMethodViewModel>> GetTopMostOfCategoryAsync();
        Task<PagedResult<BestSellerProductViewModel>> GetBestSellerProductsAsync(string fromDate, string toDate, int categoryId, string productName, int pageIndex, int pageSize);
        Task<PagedResult<ProductsHasNotBeenPurchasedViewModel>> GetListProductsHasNotBeenPurchased(string fromDate, string toDate, int categoryId, string productName, int pageIndex, int pageSize);
    }
}