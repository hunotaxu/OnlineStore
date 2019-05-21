using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;

namespace OnlineStore.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public DetailsModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public DAL.Data.Entities.Order Order { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = _orderRepository.Find(x => x.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}