using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public IndexModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [BindProperty]
        public DAL.Data.Entities.Order Order { get;set; }

        public void OnGet()
        {

        }

        public IActionResult OnGetAllPaging(byte? deliveryType, byte? orderStatus, string keyword, int pageIndex, int pageSize)
        {
            var model = _orderRepository.GetAllPaging(deliveryType, orderStatus, keyword, pageIndex, pageSize);
            return new OkObjectResult(model);
        }
    }
}