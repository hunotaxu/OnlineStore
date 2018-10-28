using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Cart
{
    public class EditModel : PageModel
    {
        public List<ItemCartViewModel> ItemCarts { get; set; }

        public EditModel()
        {
            
        }

        public void OnGet()
        {

        }
    }
}