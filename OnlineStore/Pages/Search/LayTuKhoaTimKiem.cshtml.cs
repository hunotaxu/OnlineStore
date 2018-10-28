using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Search
{
    public class LayTuKhoaTimKiemModel : PageModel
    {
        public IActionResult OnGet(string sTuKhoa)
        {
            return RedirectToPage("./SearchResult", new { sTuKhoa = sTuKhoa });
        }
    }
}