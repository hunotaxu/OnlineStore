using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Product
{
    public class DetailModel : PageModel
    {
        //IItemService _itemservice;
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly IAddressRepository _addressRepository;
        public readonly IOrderRepository _orderRepository;
        public readonly IOrderItemRepository _orderItemRepository;
        public readonly IProductImagesRepository _productImagesRepository;

        public Item Item { get; set; }
        public DAL.Data.Entities.Cart Cart { get; set; }

        public IList<CustomerCommentViewModel> CustomerCommentViewModel { get; set; }
        [BindProperty]
        public List<ProductImages> productImages { get; set; }
        
        public double Average { get; set; }
        public int ItemId;
        public double _countComment = 0;
        public int _countItemCart = 0;
        public bool isordered = false;
        public DetailModel(IProductImagesRepository productImagesRepository,IOrderItemRepository orderItemRepository,IAddressRepository addressRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager, ICartRepository cartRepository, ICartDetailRepository cartDetailRepository, IItemRepository itemRepository, ICommentRepository commentRepository, IUserRepository userRepository, DAL.EF.OnlineStoreDbContext context)
        {
            _productImagesRepository = productImagesRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _cartDetailRepository = cartDetailRepository;
            _userManager = userManager;
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
            //Comment = _commentRepository.Find(n => n.Id == id && n.IsDeleted == false);

            if (Item == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return NotFound();
            }

            ItemId = Item.Id;

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
                Average = sumEvaluation / _countComment;
            }
            else
            {
                CustomerCommentViewModel = null;
                Average = 0;
            }

            if (_userManager.GetUserAsync(HttpContext.User).Result != null)
            {
                var orders = _orderRepository.GetSome(o => o.Status == OrderStatus.Delivered);
                foreach (var order in orders)
                {
                    var address = _addressRepository.GetSome(a => a.Id == order.AddressId);
                    foreach(var _address in address)
                    {
                        if(_address.Id == order.AddressId && _address.CustomerId == _userManager.GetUserAsync(HttpContext.User).Result.Id)
                        {
                            var orderitems = _orderItemRepository.GetSome(cd => cd.IsDeleted == false && cd.ItemId == id).ToList();
                            foreach( var orderitem in orderitems)
                            {
                                if(orderitem.OrderId == order.Id)
                                {
                                    isordered = true; break;
                                }
                            }
                        }
                    }
                }

            }
            return Page();
        }

        //public IActionResult OnGetLoadPreviewImage(int? id)
        //{
        //    var productimage = _productImagesRepository.GetSome(c=>c.ItemId == 35 );
        //    if (productimage != null)
        //    {
        //        productImages = new List<ProductImages>();
        //        var productimages = productimage.Where(cd => cd.IsDeleted == false).ToList();
        //        if (productimages.Count > 0)
        //        {
        //            foreach (var item in productimages)
        //            {
        //                var productimg = new ProductImages
        //                {
        //                    Id = item.Id,
        //                    Name = item.Name,
        //                };
        //                productImages.Add(productimg);
        //            }
        //        }
        //    }
        //    return new OkObjectResult(productImages);
        //}


        //public IActionResult OnPostSaveEntity([FromBody] DAL.Data.Entities.Comment model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
        //        return new BadRequestObjectResult(allErrors);
        //    }

        //    if (model.Id == 0)
        //    {
        //        model.DateCreated = DateTime.Now;
        //        model.Id = model.Id;
        //        model.Content = model.Content;
        //        model.DateModified = DateTime.Now;
        //        model.CustomerId = model.CustomerId;
        //        model.ItemId = model.ItemId;
        //        //model.DateModified = DateTime.Now;
        //        _commentRepository.Add(model);
        //        return new OkObjectResult(model);
        //    }

        //    var comment = _commentRepository.Find(model.Id);
        //    comment.Id = model.Id;
        //    comment.Content = model.Content;
        //    comment.DateModified = DateTime.Now;

        //    return new OkObjectResult(comment);
        //}

        //public IActionResult OnPostAddToCart([FromBody] DAL.Data.Entities.CartDetail model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
        //        return new BadRequestObjectResult(allErrors);
        //    }

        //    var user = _userManager.GetUserAsync(HttpContext.User).Result;
        //    if (user != null && !_userRepository.IsAdmin(user))
        //    {
        //        var cart = _cartRepository.GetCartByCustomerId(user.Id);
        //        var _item = _itemRepository.Find(model.ItemId);
        //        if (cart == null)
        //        {
        //            var newCart = new DAL.Data.Entities.Cart
        //            {
        //                CustomerId = _userManager.GetUserAsync(HttpContext.User).Result.Id,
        //            };
        //            _cartRepository.Add(newCart);
        //            _cartDetailRepository.Add(new CartDetail
        //            {
        //                CartId = newCart.Id,
        //                ItemId = model.ItemId,
        //                Quantity = model.Quantity
        //            });
        //        }
        //        else
        //        {
        //            var cartDetails = cart.CartDetails;
        //            bool isMatch = false;
        //            foreach (var item in cart.CartDetails)
        //            {
        //                if (item.ItemId == model.ItemId)
        //                {
        //                    item.Quantity += model.Quantity;
        //                    if (item.Quantity > _item.Quantity)
        //                        return new BadRequestObjectResult("Số lượng sản phẩm trong giỏ vượt quá số lượng cho phép! " + _item.Quantity.ToString());
        //                    item.IsDeleted = false;
        //                    _cartDetailRepository.Update(item);
        //                    isMatch = true;
        //                }
        //            }
        //            if (!isMatch)
        //            {
        //                _cartDetailRepository.Add(new CartDetail
        //                {
        //                    CartId = cart.Id,
        //                    ItemId = model.ItemId,
        //                    Quantity = model.Quantity
        //                });
        //            }
        //        }
        //    }
        //    return new OkObjectResult(model);
        //}        

    }
}