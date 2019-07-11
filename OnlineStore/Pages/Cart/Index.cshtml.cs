using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlineStore.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, ICartDetailRepository cartDetailRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }

        public ActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnGetLoadCart()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            //if (!_userRepository.IsAdmin())
            //{
            var cart = _cartRepository.GetCartByCustomerId(user.Id);
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
                            Image = (item.Item.ProductImages.Count() > 0) ?
                                    $"/images/client/ProductImages/{item.Item.ProductImages?.FirstOrDefault()?.Name}" : $"/images/client/ProductImages/no-image.jpg",
                            Price = item.Item.Price,
                            ProductName = item.Item.Name,
                            Quantity = (item.Quantity < item.Item.Quantity || item.Item.Quantity <= 0) ? item.Quantity : item.Item.Quantity,
                            MaxQuantity = item.Item.Quantity
                        };
                        ItemInCarts.Add(itemCartViewModel);
                    }
                }
            }
            //}
            return new OkObjectResult(ItemInCarts);
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
                            if (item.Item.Quantity > 0)
                            {
                                var itemCartViewModel = new ItemCartViewModel
                                {
                                    ItemId = item.ItemId,
                                    //Image = $"/images/client/ProductImages/{item.Item.ProductImages?.FirstOrDefault()?.Name}",
                                    Image = (item.Item.ProductImages.Count() > 0) ?
                                        $"/images/client/ProductImages/{item.Item.ProductImages?.FirstOrDefault()?.Name}" : $"/images/client/ProductImages/no-image.jpg",
                                    Price = item.Item.Price,
                                    ProductName = item.Item.Name,
                                    Quantity = item.Quantity,
                                };
                                ItemInCarts.Add(itemCartViewModel);
                            }
                        }
                    }
                }
            }
            return new OkObjectResult(ItemInCarts);
        }

        public IActionResult OnGetConfirmOrder()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user == null)
            {
                return new BadRequestObjectResult("Giỏ hàng không tồn tại");
            }

            var cart = _cartRepository.GetCartByCustomerId(user.Id);
            if (cart == null)
            {
                return new BadRequestObjectResult("Giỏ hàng không tồn tại");
            }

            var cartDetails = cart.CartDetails.Where(x => x.IsDeleted == false);

            if (cartDetails.All(x => x.Item.Quantity <= 0))
            {
                return new BadRequestObjectResult("Tất cả các sản phẩm đã hết hàng. Vui lòng thêm sản phẩm mới vào giỏ");
            }

            return new OkResult();
        }

        public IActionResult OnPostUpdateQuantity([FromBody] CartDetail model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            var cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            if (cart != null)
            {
                var item = cart.CartDetails.FirstOrDefault(x => x.ItemId == model.ItemId);
                if (model.Quantity > item.Item.Quantity)
                {
                    return new BadRequestObjectResult("Không thể đặt hàng vượt quá số lượng cho phép");
                }
                item.Quantity = model.Quantity;
                _cartDetailRepository.Update(item);
            }
            return new OkResult();
        }

        public IActionResult OnPostDeleteItem([FromBody] CartDetail model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestResult();
            }
            var cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            var cartDetail = cart.CartDetails.FirstOrDefault(cd => cd.ItemId == model.ItemId);
            _cartDetailRepository.Delete(cartDetail);
            return new OkResult();
        }
    }
}