﻿using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;

namespace OnlineStore.Pages.Cart
{
    public class CreateModel : PageModel
    {
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICartRepository _cartRepository;

        public CreateModel(ICartDetailRepository cartDetailRepository, ICartRepository cartRepository)
        {
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult OnGetAddItem(int itemId)
        {
            Customer cus = HttpContext.Session.Get<Customer>("Customer");
            //DAL.Models.Cart cart = new DAL.Models.Cart();
            DAL.Models.Cart cart = _cartRepository.Find(m => m.CustomerId == cus.Id);
            if (cart == null)
            {
                cart = new DAL.Models.Cart
                {
                    CustomerId = cus.Id
                };
                _cartRepository.Add(cart);
            }

            CartDetail itemCart = _cartDetailRepository.Find(c => c.CartId == cart.Id && c.ItemId == itemId);

            if (itemCart != null)
            {
                itemCart.Quantity++;
                _cartDetailRepository.Update(itemCart);
            }
            else
            {
                CartDetail cartDetail = new CartDetail
                {
                    CartId = cart.Id,
                    ItemId = itemId,
                    Quantity = 1
                };
                _cartDetailRepository.Add(cartDetail);
            }
            return Redirect("/Product/Detail?id=" + itemId);
        }
    }
}