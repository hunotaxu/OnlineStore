using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        public int _numbercartitem;

        public IEnumerable<Item> Phones { get; set; }
        public IEnumerable<Item> Laptops { get; set; }
        public IEnumerable<Item> Tablets { get; set; }
        public IEnumerable<Item> Accessories { get; set; }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }

        public IndexModel(ICartRepository cartRepository, IUserRepository userRepository, ICartDetailRepository cartDetailRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;

        }

        public void OnGet()
        {
            Phones = _itemRepository.GetByCategory(1);
            Laptops = _itemRepository.GetByCategory(2);
            Tablets = _itemRepository.GetByCategory(3);
            Accessories = _itemRepository.GetByCategory(4);
            var cus = _userManager.GetUserAsync(HttpContext.User).Result;
        }
        public IActionResult OnGetLoadCartLayout()
        {
            if (_userManager.GetUserAsync(HttpContext.User).Result != null)
            {
                var cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
                if (cart != null)
                {
                    ItemInCarts = new List<ItemCartViewModel>();
                    var items = cart.CartDetails.Where(cd => cd.IsDeleted == false).ToList();
                    if (items.Count > 0)
                    {
                        foreach (var item in items)
                        {

                            var itemCartViewModel = new ItemCartViewModel
                            {
                                ItemId = item.ItemId,
                                Image = $"/images/client/ProductImages/{item.Item.ProductImages.First().Name}",
                                Price = item.Item.Price,
                                ProductName = item.Item.Name,
                                //Quantity = item.Quantity < item.Item.Quantity || item.Item.Quantity == 0) ? item.Quantity : item.Item.Quantity,
                                Quantity = item.Quantity,
                                //MaxQuantity = item.Item.Quantity
                            };
                            ItemInCarts.Add(itemCartViewModel);

                        }
                    }
                }
            }
            return new OkObjectResult(ItemInCarts);
        }

    }
}