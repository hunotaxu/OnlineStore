using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Pages.Cart
{
    public class NumberOfItemsInCartModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;

        public NumberOfItemsInCartModel(UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnGetLoadNumberItemCart()
        {
            int numberOfItemInCart = 0;

            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                DAL.Data.Entities.Cart cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
                if (cart != null && cart.CartDetails?.Any() == true)
                {
                    IEnumerable<CartDetail> items = cart.CartDetails.Where(cd => cd.IsDeleted == false);
                    foreach (var item in items)
                    {
                        if (item.Item.Quantity > 0)
                        {
                            if (item.Quantity < item.Item.Quantity)
                            {
                                numberOfItemInCart += item.Quantity;
                            }
                            else
                            {
                                numberOfItemInCart += item.Item.Quantity;
                            }
                        }
                    };
                }
            }
            return new OkObjectResult(numberOfItemInCart);
        }
    }
}