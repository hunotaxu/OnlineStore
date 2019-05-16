using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.ViewComponents
{
    public class EvaluationStarPartialViewComponent : ViewComponent
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        public Item Item { get; set; }

        public IList<CustomerCommentViewModel> CustomerCommentViewModel { get; set; }

        public double Average { get; set; }
        public double _countComment = 0;


        public EvaluationStarPartialViewComponent(IItemRepository itemRepository, ICommentRepository commentRepository, IUserRepository userRepository, DAL.EF.OnlineStoreDbContext context)
        {
            _itemRepository = itemRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            CustomerCommentViewModel = new List<CustomerCommentViewModel>();
        }

        //public IActionResult OnGet(int? id)
        //{
        //    int sumEvaluation = 0;

        //    Item = _itemRepository.Find(n => n.Id == id && n.IsDeleted == false);
        //    //Comment = _commentRepository.Find(n => n.Id == id && n.IsDeleted == false);

        //    Item.View++;
        //    _itemRepository.Update(Item);



        //    List<Comment> comments = _commentRepository.GetSome(y => y.ItemId == id).ToList();

        //    if (comments.Any())
        //    {
        //        foreach (Comment comment in comments.ToList())
        //        {
        //            sumEvaluation += comment.Evaluation;
        //            _countComment++;
        //            var cus = _userRepository.FindUser(c => c.Id == comment.CustomerId);

        //        }
        //        Average = sumEvaluation / comments.Count;
        //    }
        //    else
        //    {
        //        CustomerCommentViewModel = null;
        //        Average = 0;
        //    }
        //    return Average;
        //}
        public Task<IViewComponentResult> InvokeAsync(int id)
        {
            int sumEvaluation = 0;

            Item = _itemRepository.Find(n => n.Id == id && n.IsDeleted == false);
            //Comment = _commentRepository.Find(n => n.Id == id && n.IsDeleted == false);

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

                }
                Average = sumEvaluation / comments.Count;
            }
            else
            {
                CustomerCommentViewModel = null;
                Average = 0;
            }
            return Task.FromResult<IViewComponentResult>(View("Default"));
        }

    }
}