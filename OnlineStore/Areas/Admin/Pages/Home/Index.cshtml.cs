using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using TimiApp.Dapper.Interfaces;

namespace OnlineStore.Areas.Admin.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        public IndexModel(IReportService reportService)
        {
            _reportService = reportService;
        }
        public void OnGet()
        {
            //var email = User.GetSpecificClaim("Email");
        }
        public async Task<IActionResult> OnGetRevenue(string fromDate, string toDate)
        {
            var b = await _reportService.GetReportAsync(fromDate, toDate);
            return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
        }
    }
}