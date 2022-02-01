using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Pages.Product
{
    public class DetailModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
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
        public List<ItemViewModel> Items { get; set; }

        public int ItemId;
        public double _countComment = 0;
        public int _countItemCart = 0;
        public bool isordered = false;
        public Comment Reviewed;
        public DetailModel(IProductImagesRepository productImagesRepository, IOrderItemRepository orderItemRepository, IAddressRepository addressRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager, IItemRepository itemRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _productImagesRepository = productImagesRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _itemRepository = itemRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            CustomerCommentViewModel = new List<CustomerCommentViewModel>();
        }

        public IActionResult OnGet(int? id)
        {
            int sumEvaluation = 0;

            if (id == null)
            {
                //return BadRequest();
                return RedirectToPage("/NotFound");
            }

            Item = _itemRepository.Find(n => n.Id == id && n.IsDeleted == false);
            if (Item == null)
            {
                return RedirectToPage("/NotFound");
            }
            ItemId = Item.Id;
            Item.View++;
            _itemRepository.Update(Item);
            List<Comment> comments = _commentRepository.GetSome(y => y.ItemId == id && y.IsDeleted == false).ToList();
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
            }
            else
            {
                CustomerCommentViewModel = null;
            }
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                Reviewed = _commentRepository.Find(c => c.IsDeleted == false && c.CustomerId == user.Id && c.ItemId == id);
                var orders = _orderRepository.GetSome(o => o.Status == OrderStatus.Delivered && o.IsDeleted == false);
                foreach (var order in orders)
                {
                    var address = _addressRepository.GetSome(a => a.Id == order.AddressId && a.IsDeleted == false);
                    foreach (var _address in address)
                    {
                        if (_address.Id == order.AddressId && _address.CustomerId == user.Id)
                        {
                            var orderitems = _orderItemRepository.GetSome(cd => cd.IsDeleted == false && cd.ItemId == id).ToList();
                            foreach (var orderitem in orderitems)
                            {
                                if (orderitem.OrderId == order.Id)
                                {
                                    isordered = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return Page();
        }
    }
}