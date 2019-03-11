using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Cart
{
    public class DeleteModel : PageModel
    {
        private readonly ICartDetailRepository _cartDetailRepository;

        public DeleteModel(ICartDetailRepository cartDetailRepository)
        {
            _cartDetailRepository = cartDetailRepository;
        }

        public void OnGetDelete(int itemId, int cartId)
        {
            CartDetail cartDetail = _cartDetailRepository.Find(c => c.CartId == cartId && c.ItemId == itemId);
            _cartDetailRepository.Delete(cartDetail);
        }
    }
}