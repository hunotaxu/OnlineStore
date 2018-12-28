using System.Collections.Generic;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Cart
{
    public class EditModel : PageModel
    {
        private readonly ICartDetailRepository _cartDetailRepository;

        public EditModel(ICartDetailRepository cartDetailRepository)
        {
            _cartDetailRepository = cartDetailRepository;
        }

        public void OnGetUpdateQuantity(int itemId, int cartId, int newQuantity)
        {
            CartDetail cartDetail = _cartDetailRepository.Find(c => c.CartId == cartId && c.ItemId == itemId);
            cartDetail.Quantity = newQuantity;
            _cartDetailRepository.Update(cartDetail);
        }
    }
}