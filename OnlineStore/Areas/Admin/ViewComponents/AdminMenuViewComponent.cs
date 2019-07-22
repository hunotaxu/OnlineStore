using DAL.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Commons;
using System.Threading.Tasks;
using OnlineStore.Areas.Admin.ViewModels;
using DAL.Repositories;

namespace OnlineStore.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        public AdminMenuViewComponent(UserManager<ApplicationUser> userManager,
            IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            //var isProductManager = await _userManager.IsInRoleAsync(user, CommonConstants.ProductManagerRoleName);
            var numberOfOrdersInProgress = _orderRepository.GetSome(o => o.IsDeleted == false && o.Status == DAL.Data.Enums.OrderStatus.Pending).Count();
            var viewModel = new AdminMenuViewModel
            {
                NumberOfOrdersInProgress = numberOfOrdersInProgress,
                IsProductManager = user != null ? await _userManager.IsInRoleAsync(user, CommonConstants.ProductManagerRoleName) : false,
                IsStoreOwner = user != null ? await _userManager.IsInRoleAsync(user, CommonConstants.StoreOwnerRoleName) : false,
                IsOrderManager = user != null ? await _userManager.IsInRoleAsync(user, CommonConstants.OrderManagerRoleName) : false
            };
            return View(viewModel);
        }
    }
}
