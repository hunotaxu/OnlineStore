using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using System.Collections.Generic;
using Utilities.Commons;

namespace OnlineStore.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository)
        {
            _itemRepository = itemRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }

        public ActionResult OnGet()
        {
            var cus = _userManager.GetUserAsync(HttpContext.User).Result;
            if (cus == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Cart/Index" });
            }
            return Page();
        }

        public IActionResult OnGetLoadCart()
        {
            var cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            if (cart != null)
            {
                ItemInCarts = new List<ItemCartViewModel>();
                foreach (var item in cart.CartDetails)
                {
                    var itemCartViewModel = new ItemCartViewModel
                    {
                        ItemId = item.ItemId,
                        Image = $"/images/client/ProductImages/{item.Item.Image}",
                        Price = item.Item.Price,
                        ProductName = item.Item.Name,
                        Quantity = item.Quantity,
                    };
                    ItemInCarts.Add(itemCartViewModel);
                }
            }
            return new OkObjectResult(ItemInCarts);
        }
        ///// <summary>
        ///// Remove all products in cart
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult ClearCart()
        //{
        //    HttpContext.Session.Remove(CommonConstants.CartSession);
        //    return new OkObjectResult("OK");
        //}

        ///// <summary>
        ///// Add product to cart
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <param name="quantity"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult AddToCart(int itemId, int quantity)
        //{
        //    //Get product detail
        //    var item = _itemservice.GetById(itemId);

        //    //Get session with item list from cart
        //    var session = HttpContext.Session.Get<List<ItemCartViewModel>>(CommonConstants.CartSession);
        //    if (session != null)
        //    {
        //        //Convert string to list object
        //        bool hasChanged = false;

        //        //Check exist with item product id
        //        if (session.Any(x => x.Item.Id == itemId))
        //        {
        //            foreach (var _item in session)
        //            {
        //                //Update quantity for product if match product id
        //                if (_item.Item.Id == itemId)
        //                {
        //                    _item.Quantity += quantity;
        //                    _item.Price = item.PromotionPrice ?? item.Price;
        //                    hasChanged = true;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            session.Add(new ItemCartViewModel()
        //            {
        //                Item = item,
        //                Quantity = quantity,
        //                Price = item.PromotionPrice ?? item.Price
        //            });
        //            hasChanged = true;
        //        }

        //        //Update back to cart
        //        if (hasChanged)
        //        {
        //            HttpContext.Session.Set(CommonConstants.CartSession, session);
        //        }
        //    }
        //    else
        //    {
        //        //Add new cart
        //        var cart = new List<ItemCartViewModel>();
        //        cart.Add(new ItemCartViewModel()
        //        {
        //            Item = item,
        //            Quantity = quantity,
        //            Price = item.PromotionPrice ?? item.Price
        //        });
        //        HttpContext.Session.Set(CommonConstants.CartSession, cart);
        //    }
        //    return new OkObjectResult(itemId);
        //}

        ///// <summary>
        ///// Remove a product
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public IActionResult RemoveFromCart(int productId)
        //{
        //    var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
        //    if (session != null)
        //    {
        //        bool hasChanged = false;
        //        foreach (var item in session)
        //        {
        //            if (item.Product.Id == productId)
        //            {
        //                session.Remove(item);
        //                hasChanged = true;
        //                break;
        //            }
        //        }
        //        if (hasChanged)
        //        {
        //            HttpContext.Session.Set(CommonConstants.CartSession, session);
        //        }
        //        return new OkObjectResult(productId);
        //    }
        //    return new EmptyResult();
        //}

        ///// <summary>
        ///// Update product quantity
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <param name="quantity"></param>
        ///// <returns></returns>
        //public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        //{
        //    var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
        //    if (session != null)
        //    {
        //        bool hasChanged = false;
        //        foreach (var item in session)
        //        {
        //            if (item.Product.Id == productId)
        //            {
        //                var product = _productService.GetById(productId);
        //                item.Product = product;
        //                item.Size = _billService.GetSize(size);
        //                item.Color = _billService.GetColor(color);
        //                item.Quantity = quantity;
        //                item.Price = product.PromotionPrice ?? product.Price;
        //                hasChanged = true;
        //            }
        //        }
        //        if (hasChanged)
        //        {
        //            HttpContext.Session.Set(CommonConstants.CartSession, session);
        //        }
        //        return new OkObjectResult(productId);
        //    }
        //    return new EmptyResult();
        //}
    }
}