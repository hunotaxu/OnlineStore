using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Product
{
    public class AddToCartModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IItemRepository _itemRepository;
        public AddToCartModel(UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ICartRepository cartRepository,
            ICartDetailRepository cartDetailRepository,
            IItemRepository itemRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _cartDetailRepository = cartDetailRepository;
            _itemRepository = itemRepository;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostAddToCart([FromBody] CartDetail model)
        {
            if (!ModelState.IsValid)
            {
                //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult("Đã xãy ra lỗi");
            }

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var cart = _cartRepository.GetCartByCustomerId(user.Id);
            var _item = _itemRepository.Find(model.ItemId);

            if (model.Quantity == 0)
            {
                return new BadRequestObjectResult("Số lượng sản phẩm không hợp lệ");
            }

            if (model.Quantity > _item.Quantity)
            {
                return new BadRequestObjectResult("Số lượng sản phẩm trong giỏ vượt quá số lượng cho phép là: " + _item.Quantity.ToString());
            }

            if (cart == null)
            {
                var newCart = new DAL.Data.Entities.Cart
                {
                    CustomerId = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                };
                _cartRepository.Add(newCart);
                
                _cartDetailRepository.Add(new CartDetail
                {
                    CartId = newCart.Id,
                    ItemId = model.ItemId,
                    Quantity = model.Quantity
                });
            }
            else
            {
                var cartDetails = cart.CartDetails.Where(cd => cd.IsDeleted == false);
                bool isMatch = false;
                foreach (var item in cart.CartDetails)
                {
                    if (item.ItemId == model.ItemId)
                    {
                        item.Quantity = item.IsDeleted == true ? model.Quantity : item.Quantity + model.Quantity;
                        if (_item.Quantity == 0)
                        {
                            return new BadRequestObjectResult("Sản phẩm đã hết hàng");
                        }
                        if (item.Quantity > _item.Quantity)
                        {
                            return new BadRequestObjectResult("Số lượng sản phẩm trong giỏ vượt quá số lượng cho phép là: " + _item.Quantity.ToString());
                        }
                        item.IsDeleted = false;
                        _cartDetailRepository.Update(item);
                        isMatch = true;
                    }
                }
                if (!isMatch)
                {
                    _cartDetailRepository.Add(new CartDetail
                    {
                        CartId = cart.Id,
                        ItemId = model.ItemId,
                        Quantity = model.Quantity
                    });
                }
            }
            return new OkObjectResult(model);
        }
    }
}