using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Pages.Order
{
    public class MyOrderModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public MyOrderViewModel MyOrderViewModel { get; set; }
        public MyOrderModel(IOrderRepository orderRepository,
            IItemRepository itemRepository,
            UserManager<ApplicationUser> userManager)
        {
            MyOrderViewModel = new MyOrderViewModel();
            _orderRepository = orderRepository;
            _userManager = userManager;
            _itemRepository = itemRepository;
        }
        public ActionResult OnGet(int orderId)
        {
            var order = _orderRepository.GetSome(x => x.Id == orderId && x.IsDeleted == false).FirstOrDefault();
            var customer = _userManager.GetUserAsync(HttpContext.User).Result;
            if (customer == null)
            {
                return RedirectToPage("/NotFound");
            }
            if (order.Address.CustomerId != customer.Id)
            {
                return RedirectToPage("/NotFound");
            }
            MyOrderViewModel.Order = order;
            return Page();
        }

        public IActionResult OnPostCancelOrder([FromBody] DAL.Data.Entities.Order model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //return new BadRequestObjectResult(allErrors);
                return new BadRequestObjectResult("Đã có lỗi xãy ra");
            }

            if (model.Id == 0)
            {
                return new BadRequestObjectResult("Mã đơn hàng không hợp lệ");
            }
            else
            {
                using (var context = new OnlineStoreDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var order = _orderRepository.Find(model.Id);
                            order.DateModified = DateTime.Now;
                            order.Status = OrderStatus.Canceled;
                            _orderRepository.Update(order);
                            var orderItems = order.OrderItems?.Where(oi => oi.IsDeleted == false);
                            if (orderItems?.Any() == false)
                            {
                                transaction.Rollback();
                                return new BadRequestObjectResult("Hủy đơn hàng không thành công");
                            }

                            foreach (var orderItem in orderItems)
                            {
                                orderItem.Item.Quantity += orderItem.Quantity;
                                _itemRepository.Update(orderItem.Item);
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return new BadRequestObjectResult("Hủy đơn hàng không thành công");
                        }
                    }
                }

            }

            return new OkObjectResult(model);
        }
    }
}