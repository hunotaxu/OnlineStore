using System;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Extensions;
using Utilities.Commons;

namespace OnlineStore.ViewComponents
{
    public class CartPartialViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepository;

        public CartPartialViewComponent(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            if (SumQuantity() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return Task.FromResult<IViewComponentResult>(View("Default"));
            }
            ViewBag.TongSoLuong = SumQuantity();
            ViewBag.TongTien = SumTotalAmount();
            return Task.FromResult<IViewComponentResult>(View("Default"));
        }

        public int SumQuantity()
        {
            Cart cart;
            var cus = HttpContext.Session.Get<ApplicationUser>(CommonConstants.UserSession);
            if (cus != null)
            {
                cart = GetCart(cus.Id);
            }
            else
            {
                ////cart = HttpContext.Session.Get<Cart>("Cart");
                //if (cart == null)
                //{
                    return 0;
                //}
            }
            return _cartRepository.GetQuantity(cart.Id);
        }

        private decimal SumTotalAmount()
        {
            Cart cart;
            var user = HttpContext.Session.Get<ApplicationUser>(CommonConstants.UserSession);
            if (user != null)
            {
                cart = GetCart(user.Id);
            }
            else
            {
                cart = HttpContext.Session.Get<Cart>("Cart");
                if (cart == null)
                {
                    return 0;
                }
            }

            return _cartRepository.GetTotalAmount(cart.Id);
        }

        public Cart GetCart(Guid customerId)
        {
            var cart = _cartRepository.GetCartByCustomerId(customerId);
            //HttpContext.Session.Set("Cart", cart);
            return cart;
        }
    }
}