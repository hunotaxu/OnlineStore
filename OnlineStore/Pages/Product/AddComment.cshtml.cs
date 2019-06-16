using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Pages.Product
{
    public class AddCommentModel : PageModel
    {
        private readonly ICommentRepository _commentRepository;
        public AddCommentModel(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
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
                return new OkObjectResult(model);
            }

            var comment = _commentRepository.Find(model.Id);
            comment.Id = model.Id;
            comment.Content = model.Content;
            comment.DateModified = DateTime.Now;

            return new OkObjectResult(comment);
        }
    }
}