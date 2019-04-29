//using System.Collections.Generic;
//using System.Threading.Tasks;
//using DAL.Data.Entities;
//using DAL.Models;
//using DAL.Repositories;
//using Microsoft.AspNetCore.Mvc;


//namespace OnlineStore.ViewComponents
//{
//    public class CommentPartialViewComponent : ViewComponent
//    {
//        private readonly ICommentRepository _commentRepository;

//        public CommentPartialViewComponent(ICommentRepository commentRepository)
//        {
//            _commentRepository = commentRepository;
//            //_itemRepository = itemRepository;
//        }
//        public Task<IViewComponentResult> InvokeAsync()
//        {
//            IEnumerable<Comment> comments = _commentRepository.GetAll();

//            return Task.FromResult<IViewComponentResult>(View("Default", comments));
//        }
//    }
//}
