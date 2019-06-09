using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TimiApp.Dapper.Interfaces;

namespace OnlineStore.Areas.Admin.Pages.Reports
{
    public class TopSellingProductsModel : PageModel
    {
        private readonly IReportService _reportService;
        public TopSellingProductsModel(IReportService reportService)
        {
            _reportService = reportService;
        }
        public void OnGet()
        {
            
        }
        //public async Task<IActionResult> OnGetBestSellerProductAsync(string fromDate, string toDate, int categoryId, string productName)
        //public async Task<IActionResult> OnGetBestSellerProductAsync(string fromDate, string toDate)
        public async Task<IActionResult> OnGetBestSellerProduct(string fromDate, string toDate, int categoryId, string productName, int pageIndex, int pageSize)
        {
            //var products = await _reportService.GetBestSellerProductsAsync(fromDate, toDate, categoryId, productName);
            var products = await _reportService.GetBestSellerProductsAsync(fromDate, toDate, categoryId, productName, pageIndex, pageSize);
            return new OkObjectResult(products);
        }
        public async Task<IActionResult> OnGetRevenue(string fromDate, string toDate)
        {
            return new OkObjectResult(await _reportService.GetRevenueReportAsync(fromDate, toDate));
        }
    }
}