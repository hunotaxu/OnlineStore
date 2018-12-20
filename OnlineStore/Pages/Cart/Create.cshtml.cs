using System.Linq;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;

namespace OnlineStore.Pages.Cart
{
    public class CreateModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICartDetailRepository _cartDetailRepository;


        public CreateModel(IItemRepository itemRepository, ICartDetailRepository cartDetailRepository)
        {
            _itemRepository = itemRepository;
            _cartDetailRepository = cartDetailRepository;
        }

        public void OnGet(string itemId)
        {

        }

        public IActionResult OnGetAddItem(int itemId)
        {
            Customer cus = HttpContext.Session.Get<Customer>("Customer");
            CartDetail itemCart = cus.Carts.CartDetails.SingleOrDefault(c => c.ItemId == itemId);
            if (itemCart != null)
            {
                itemCart.Quantity++;
            }
            else
            {
                Item item = _itemRepository.Find(itemId);
                CartDetail cartDetail = new CartDetail
                {
                    CartId = cus.Carts.Id,
                    ItemId = itemId,
                    Quantity = 1
                };
                _cartDetailRepository.Add(cartDetail);
            }
            return Redirect("/Product/Detail?id=" + itemId);
        }
    }
}