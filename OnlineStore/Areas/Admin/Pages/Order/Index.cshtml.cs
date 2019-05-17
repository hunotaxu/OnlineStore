using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;
using System.Linq;
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

        public IActionResult OnGetAllPaging(int pageIndex, int pageSize)
        {
            //var admin = HttpContext.Session.Get<ApplicationUser>(CommonConstants.UserSession);
            //if (admin == null || !_userRepository.IsProductManager(admin.UserName))
            //{
            //    return new JsonResult(new { authenticate = false });
            //}
            var model = _orderRepository.GetAllPaging(pageIndex, pageSize);
            //var itemsPagination = _mapperConfiguration.CreateMapper().Map<PagedResult<ItemViewModel>>(model);
            return new OkObjectResult(model);
        }
    }
}