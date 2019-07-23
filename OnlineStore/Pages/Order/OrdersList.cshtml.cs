using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Utilities.Commons;

namespace OnlineStore.Pages.Order
{
    public class OrdersListModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        //public List<DAL.Data.Entities.Order> Orders { get; set; }
        public PaginatedList<DAL.Data.Entities.Order> Orders { get; set; }
        public OrdersListModel(IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager)
        {
            //Orders = new List<DAL.Data.Entities.Order>();
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public void OnGet(int? pageIndex)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                var orders = _orderRepository.GetSome(o => o.IsDeleted == false && o.Address.CustomerId == user.Id).OrderByDescending(o => o.Id);
                Orders = PaginatedList<DAL.Data.Entities.Order>.CreateAsync(orders, pageIndex ?? 1, CommonConstants.PageSize);
            }
        }
    }
}