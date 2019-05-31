using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Product
{
    public class DetailModel : PageModel
    {
        //IItemService _itemservice;
        private readonly IItemRepository _itemRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        public Item Item { get; set; }

        public IList<CustomerCommentViewModel> CustomerCommentViewModel { get; set; }

        public double Average { get; set; }
        public int ItemId;
        public double _countComment = 0;


       
        public DetailModel(IItemRepository itemRepository, ICommentRepository commentRepository, IUserRepository userRepository, DAL.EF.OnlineStoreDbContext context)
        {
            //_itemservice = itemservice;
            _itemRepository = itemRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            CustomerCommentViewModel = new List<CustomerCommentViewModel>();
        }

        public IActionResult OnGet(int? id)
        {
            // int TongDiem = 0;
            int sumEvaluation = 0;
            
            //Kiểm tra tham số truyền vào có rỗng hay không
            if (id == null)
            {
                return BadRequest();
            }

            //Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng

            Item = _itemRepository.Find(n => n.Id == id && n.IsDeleted == false);
            ItemId = Item.Id;
            //Comment = _commentRepository.Find(n => n.Id == id && n.IsDeleted == false);

            if (Item == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return NotFound();
            }

            Item.View++;
            _itemRepository.Update(Item);


            
            List<Comment> comments = _commentRepository.GetSome(y => y.ItemId == id).ToList();
            
            if (comments.Any())
            {
                foreach (Comment comment in comments.ToList())
                {
                    sumEvaluation += comment.Evaluation;
                    _countComment++;
                    var cus = _userRepository.FindUser(c => c.Id == comment.CustomerId);

                    CustomerCommentViewModel.Add(new CustomerCommentViewModel()
                    {
                        CommentId = comment.Id,
                        Content = comment.Content,
                        CustomerId = comment.CustomerId,
                        CustomerName = cus.Name,
                        CustomerAvatar = cus.Avatar,
                        Evaluation = comment.Evaluation,
                        ItemId = comment.ItemId,
                        DateCreated = comment.DateCreated,
                        DateModified = comment.DateModified

                    });
                }
                Average = sumEvaluation / comments.Count;
            }
            else
            {
                CustomerCommentViewModel = null;
                Average = 0;
            }
            return Page();

        }
        public IActionResult OnPostSaveEntity([FromBody] DAL.Data.Entities.Comment model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
                                                                                                                                                                                                                                                                                                                                                                                                                    
            if (model.Id == 0)
            {
                model.DateCreated = DateTime.Now;
                model.Id = model.Id;
                model.Content = model.Content;
                model.DateModified = DateTime.Now;
                model.CustomerId = model.CustomerId;                
                model.ItemId = model.ItemId;
                //model.DateModified = DateTime.Now;
                _commentRepository.Add(model);
                return new OkObjectResult(model);
            }

            var comment = _commentRepository.Find(model.Id);
            comment.Id = model.Id;
            comment.Content = model.Content;
            comment.DateModified = DateTime.Now;

            return new OkObjectResult(comment);
        }



        

        //#region AJAX Request
        ///// <summary>
        ///// Get list item
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult GetCart()
        //{
        //    var session = HttpContext.Session.Get<List<ItemCartViewModel>>(CommonConstants.CartSession);
        //    if (session == null)
        //        session = new List<ItemCartViewModel>();
        //    return new OkObjectResult(session);
        //}
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

        //    #endregion
        //}
    }
}