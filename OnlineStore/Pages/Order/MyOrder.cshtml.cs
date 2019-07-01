using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;
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
            MyOrderViewModel.OrderItems = order.OrderItems.ToList();
        }
    }
}