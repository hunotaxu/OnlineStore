using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Pages.Product
{
    public class AddCommentModel : PageModel
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IItemRepository _itemRepository;

        public AddCommentModel(IItemRepository itemRepository, ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _itemRepository = itemRepository;
        }
        public void OnGet()
        {

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
                _commentRepository.Add(model);

                //Tính trung bình cộng đánh giá
                var item = _itemRepository.Find(model.ItemId);
                if (item.Comments != null && item.Comments.Count() != 0)
                {
                    decimal avg = Convert.ToDecimal(string.Format("{0:0.##}", (decimal)item.Comments.Sum(x => x.Evaluation) / item.Comments.Count()));
                    item.Id = model.ItemId;
                    item.AverageEvaluation = avg;
                    _itemRepository.Update(item);
                }
                return new OkObjectResult(model);
            }
            //case comment đã tồn tại
            var comment = _commentRepository.Find(model.Id);
            comment.Id = model.Id;
            comment.Content = model.Content;
            comment.DateModified = DateTime.Now;
            return new OkObjectResult(comment);
        }
    }
}