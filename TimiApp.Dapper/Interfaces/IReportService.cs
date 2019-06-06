using System.Collections.Generic;
using System.Threading.Tasks;
using TimiApp.Dapper.ViewModels;

namespace TimiApp.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetRevenueReportAsync(string fromDate, string toDate);
        Task<IEnumerable<BestSellerOfCategoryViewModel>> GetBestSellerOfCategory();
        Task<IEnumerable<MostDeliveryMethodViewModel>> GetTopMostOfCategoryAsync();
    }
}