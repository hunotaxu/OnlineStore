using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TimiApp.Dapper.Interfaces;

namespace OnlineStore.Areas.Admin.Pages.Reports
{
    public class ProductsNotPurchasedModel : PageModel
    {
        private readonly IReportService _reportService;
        public ProductsNotPurchasedModel(IReportService reportService)
        {
            _reportService = reportService;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnGetListProducts(string fromDate, string toDate, int categoryId, string productName, int pageIndex, int pageSize)
        {
            var products = await _reportService.GetListProductsHasNotBeenPurchased(fromDate, toDate, categoryId, productName, pageIndex, pageSize);
            return new OkObjectResult(products);
        }
    }
}