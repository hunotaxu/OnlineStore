using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Order
{
    public class ConfirmAndThanksForOrderModel : PageModel
    {
        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public void OnGet(int orderId, string email)
        {
            OrderId = orderId;
            Email = email;
        }
    }
}