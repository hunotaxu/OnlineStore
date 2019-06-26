using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Product
{
    public class SearchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public SearchProductViewModel SearchProductViewModel { get; set; }
        public void OnGet()
        {

        }
    }
}