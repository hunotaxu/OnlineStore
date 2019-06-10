using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutModel(IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, ICartDetailRepository cartDetailRepository, IUserAddressRepository userAddressRepository
            , IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _userAddressRepository = userAddressRepository;
        }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }
        [BindProperty]
        public List<UserAddressViewModel> UserAddresses { get; set; }


        public ActionResult OnGet()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user == null || _userRepository.IsAdmin(user))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Cart/Index" });
            }
            return Page();
        }

        public IActionResult OnGetLoadTmpCart()
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
                            Image = $"/images/client/ProductImages/{item.Item.Image}",
                            Price = item.Item.Price,
                            ProductName = item.Item.Name,
                            Quantity = (item.Quantity < item.Item.Quantity || item.Item.Quantity == 0) ? item.Quantity : item.Item.Quantity,
                            MaxQuantity = item.Item.Quantity
                        };
                        ItemInCarts.Add(itemCartViewModel);
                    }
                }
            }
            return new OkObjectResult(ItemInCarts);
        }
        public IActionResult OnGetLoadAddress()
        {
            var useraddress = _userAddressRepository.GetByUserId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            if (useraddress != null)
            {
                UserAddresses = new List<UserAddressViewModel>();
                var items = useraddress.Where(cd => cd.IsDeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var userAddress = new UserAddressViewModel
                        {
                            AddressId = item.AddressId,
                            CustomerId = item.CustomerId,
                            PhoneNumber = item.PhoneNumber,
                            RecipientName = item.RecipientName,
                            Province = item.Address.Province,
                            District = item.Address.District,
                            Ward = item.Address.Ward,
                            Detail = item.Address.Detail
                        };
                        UserAddresses.Add(userAddress);
                    }
                }
            }
            return new OkObjectResult(UserAddresses);
        }
    }
}