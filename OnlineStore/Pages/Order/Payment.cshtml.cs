using System;
using System.Collections.Generic;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Order
{
    public class PaymentModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IItemRepository _itemRepository;
        private ILineItemRepository _lineItemRepository;

        public IList<CustomerCartViewModel> CustomerCartViewModel { get; set; }
        public decimal ShippingFee { get; set; }
        public int AddressId { get; set; }

        public PaymentModel(IOrderRepository orderRepository, ICartDetailRepository cartDetailRepository, ICartRepository cartRepository, IItemRepository itemRepository, ILineItemRepository lineItemRepository)
        {
            _cartDetailRepository = cartDetailRepository;
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _cartRepository = cartRepository;
            _lineItemRepository = lineItemRepository;
            CustomerCartViewModel = new List<CustomerCartViewModel>();
        }

        public IActionResult OnGet(decimal shippingFee, int addressId)
        {
            Customer cus = HttpContext.Session.Get<Customer>("Customer");
            if (cus == null)
            {
                return RedirectToPage("/Home/Index");
            }
            ShippingFee = shippingFee;
            AddressId = addressId;
            DAL.Models.Cart cart = _cartRepository.GetCartByCustomerId(cus.Id);
            IEnumerable<CartDetail> cartItems = _cartDetailRepository.GetItems(c => c.CartId == cart.Id);
            foreach (CartDetail itemInCart in cartItems)
            {
                Item item = _itemRepository.Find(itemInCart.ItemId);
                CustomerCartViewModel customerCart = new CustomerCartViewModel()
                {
                    CartId = cart.Id,
                    Image = item.Image,
                    ItemId = item.Id,
                    ItemName = item.Name,
                    Price = item.Price,
                    Quantity = itemInCart.Quantity
                };
                customerCart.Subtotal = customerCart.Quantity * customerCart.Price;
                CustomerCartViewModel.Add(customerCart);
            }
            return Page();
        }

        public IActionResult OnGetAction(int customerId, decimal shippingFee, int addressId)
        {
            int a = AddressId;
            DAL.Models.Order order = new DAL.Models.Order
            {
                CustomerId = customerId,
                Status = StatusOrder.Pending,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(3),
                ShippingFee = shippingFee,
                AddressId = addressId
            };
            _orderRepository.Add(order);
            DAL.Models.Cart cart = _cartRepository.Find(c => c.CustomerId == customerId);
            IEnumerable<CartDetail> items = _cartDetailRepository.GetItems(i => i.CartId == cart.Id);
            foreach (CartDetail item in items)
            {
                LineItem lineItem = new LineItem
                {
                    Quantity = item.Quantity,
                    ItemId = item.ItemId,
                    OrderId = order.Id,
                    Amount = _itemRepository.Find(item.ItemId).Price * item.Quantity
                };
                _lineItemRepository.Add(lineItem);
            }
            _cartDetailRepository.DeleteRange(items);
            return RedirectToPage("./Success");
        }

        public void OnPost()
        {

        }
    }

}