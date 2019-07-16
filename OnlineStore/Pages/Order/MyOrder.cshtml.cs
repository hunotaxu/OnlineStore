using DAL.Data.Enums;
using DAL.EF;
using DAL.Repositories;
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
        [BindProperty]
        public MyOrderViewModel MyOrderViewModel { get; set; }
        public MyOrderModel(IOrderRepository orderRepository,
            IItemRepository itemRepository)
        {
            MyOrderViewModel = new MyOrderViewModel();
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
        }
        public void OnGet(int orderId)
        {
            var order = _orderRepository.Find(orderId);
            MyOrderViewModel.Order = order;
        }

        public IActionResult OnPostCancelOrder([FromBody] DAL.Data.Entities.Order model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
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