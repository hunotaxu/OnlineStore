using System;
using System.Collections.Generic;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities.Extensions;
using OnlineStore.Models.ViewModels;
using Utilities.Commons;
using System.Linq;

namespace OnlineStore.Pages.Order
{
    public class PaymentModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private IOrderItemRepository _orderItemRepository;

        public IList<CustomerCartViewModel> CustomerCartViewModel { get; set; }
        public decimal ShippingFee { get; set; }
        public int AddressId { get; set; }

        public PaymentModel(UserManager<ApplicationUser> userManager, IUserRepository userRepository, IOrderRepository orderRepository, ICartDetailRepository cartDetailRepository, ICartRepository cartRepository, IItemRepository itemRepository, IOrderItemRepository orderItemRepository)
        {
            _cartDetailRepository = cartDetailRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _itemRepository = itemRepository;
            _cartRepository = cartRepository;
            _orderItemRepository = orderItemRepository;
            CustomerCartViewModel = new List<CustomerCartViewModel>();
        }

        public IActionResult OnGet(decimal shippingFee, int addressId)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            //if (user == null || _userRepository.IsAdmin(user))
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Cart/Index" });
            //}
            ShippingFee = shippingFee;
            AddressId = addressId;
            DAL.Data.Entities.Cart cart = _cartRepository.GetCartByCustomerId(user.Id);
            IEnumerable<CartDetail> cartItems = _cartDetailRepository.GetSome(c => c.CartId == cart.Id);
            foreach (CartDetail itemInCart in cartItems)
            {
                Item item = _itemRepository.Find(itemInCart.ItemId);
                CustomerCartViewModel customerCart = new CustomerCartViewModel()
                {
                    CartId = cart.Id,
                    Image = item.ProductImages.First().Name,
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

        public IActionResult OnGetAction(Guid customerId, decimal shippingFee, int addressId)
        {
            int a = AddressId;
            DAL.Data.Entities.Order order = new DAL.Data.Entities.Order
            {
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(3),
                ShippingFee = shippingFee,
                AddressId = addressId
            };
            _orderRepository.Add(order);
            DAL.Data.Entities.Cart cart = _cartRepository.Find(c => c.CustomerId == customerId);
            IEnumerable<CartDetail> items = _cartDetailRepository.GetSome(i => i.CartId == cart.Id);
            foreach (CartDetail item in items)
            {
                OrderItem OrderItem = new OrderItem
                {
                    Quantity = item.Quantity,
                    ItemId = item.ItemId,
                    OrderId = order.Id,
                    Amount = _itemRepository.Find(item.ItemId).Price * item.Quantity
                };
                _orderItemRepository.Add(OrderItem);
            }
            _cartDetailRepository.DeleteRange(items);
            return RedirectToPage("./Success");
        }

        public void OnPost()
        {

        }
    }

}