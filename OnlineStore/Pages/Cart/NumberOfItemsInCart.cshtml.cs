using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Cart
{
    public class NumberOfItemsInCartModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        public NumberOfItemsInCartModel(UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ICartRepository cartRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
        }
        public void OnGet()
        {

        }

        public IActionResult OnGetLoadNumberItemCart()
        {
            var itemnumbercart = 0;

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                var cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
                if (cart != null)
                {
                    var items = cart.CartDetails.Where(cd => cd.IsDeleted == false).ToList();
                    foreach (var item in items)
                    {
                        if (item.Item.Quantity > 0)
                        {
                            itemnumbercart += item.Quantity;
                        }
                    };
                }
            }
            return new OkObjectResult(itemnumbercart);

        }
    }
}