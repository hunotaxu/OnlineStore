using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Product
{
    public class DetailModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        public Item Item { get; set; }
        public List<CustomerCommentViewModel> Customers { get; set; }
        public double Average { get; set; }

        public DetailModel(IItemRepository itemRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
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
            //SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);

            Item = _itemRepository.Find(n => n.Id == id && n.IsDeleted == false);
            
            if (Item == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return NotFound();
            }

            Item.View++;
            _itemRepository.Update(Item);
            List<Comment> comments = _commentRepository.GetSome(n => n.ItemId == id).ToList();

            if (comments.Any())
            {
                foreach (Comment comment in comments.ToList())
                {
                    sumEvaluation += comment.Evaluation;

                    var cus = _userRepository.FindUser(c => c.Id == comment.CustomerId);

                    Customers.Add(new CustomerCommentViewModel()
                    {
                        CommentId = comment.Id,
                        Content = comment.Content,
                        CustomerId = comment.CustomerId,
                        //CustomerName = cus.FirstName + ' ' + cus.LastName,
                        Evaluation = comment.Evaluation,
                        ItemId = comment.ItemId,
                        Time = DateTime.Now
                    });
                }
                Average = sumEvaluation / comments.Count;
            }
            else
            {
                Customers = null;
                Average = 0;
            }
            return Page();
        }
    }
}