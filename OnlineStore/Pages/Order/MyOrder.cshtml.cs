using DAL.Data.Enums;
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
        [BindProperty]
        public MyOrderViewModel MyOrderViewModel { get; set; }
        public MyOrderModel(IOrderRepository orderRepository)
        {
            MyOrderViewModel = new MyOrderViewModel();
            _orderRepository = orderRepository;
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
                var order = _orderRepository.Find(model.Id);
                order.DateModified = DateTime.Now;
                order.Status = OrderStatus.Canceled;
                _orderRepository.Update(order);
            }

            return new OkObjectResult(model);
        }
    }
}