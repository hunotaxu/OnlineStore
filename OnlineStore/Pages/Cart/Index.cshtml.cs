using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models.ViewModels.Item;
using OnlineStore.Interfaces;

using Utilities.Commons;

namespace OnlineStore.Pages.Order
{
    public class IndexModel : PageModel
    {
        IItemService _itemservice;
        private readonly IItemRepository _itemRepository;


        public IndexModel(IItemService itemservice)
        {
            _itemservice = itemservice;
        }

        #region AJAX Request
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ItemCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ItemCartViewModel>();
            return new OkObjectResult(session);
        }
        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int itemId, int quantity)
        {
            //Get product detail
            var item = _itemservice.GetById(itemId);

            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<ItemCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false; 

                //Check exist with item product id
                if (session.Any(x => x.Item.Id == itemId))
                {
                    foreach (var _item in session)
                    {
                        //Update quantity for product if match product id
                        if (_item.Item.Id == itemId)
                        {
                            _item.Quantity += quantity;
                            _item.Price = item.PromotionPrice ?? item.Price;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new ItemCartViewModel()
                    {
                        Item = item,
                        Quantity = quantity,
                        Price = item.PromotionPrice ?? item.Price
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ItemCartViewModel>();
                cart.Add(new ItemCartViewModel()
                {
                    Item = item,
                    Quantity = quantity,                    
                    Price = item.PromotionPrice ?? item.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(itemId);
        }

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

        #endregion
        //}
    }
}