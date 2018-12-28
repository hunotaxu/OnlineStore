using System;
using System.Collections.Generic;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Order
{
    public class PaymentModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICartRepository _cartRepository;

        public PaymentModel(IOrderRepository orderRepository, ICartDetailRepository cartDetailRepository, ICartRepository cartRepository)
        {
            _cartDetailRepository = cartDetailRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetAction(int customerId)
        {
            DAL.Models.Order order = new DAL.Models.Order
            {
                CustomerId = customerId,
                Status = StatusOrder.Pending,
                OrderDate = DateTime.Now
            };
            _orderRepository.Add(order);
            DAL.Models.Cart cart = _cartRepository.Find(c => c.CustomerId == customerId);
            IEnumerable<CartDetail> items = _cartDetailRepository.GetItems(i => i.CartId == cart.Id);
            _cartDetailRepository.DeleteRange(items);
            return RedirectToPage("./Success");
        }

        public void OnPost()
        {

        }
    }

}