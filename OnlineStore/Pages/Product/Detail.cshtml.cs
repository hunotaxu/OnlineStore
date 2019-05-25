using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            //var comment = new Comment
            //{
            //    Name = Input.Name,
            //    DOB = Input.DOB,
            //    UserName = Input.PhoneNumber,
            //    Email = Input.Email,
            //    PhoneNumber = Input.PhoneNumber,
            //    Gender = Input.Gender
            //};

            //var result = await _userManager.CreateAsync(user, Input.Password);
            //if (result.Succeeded)
            //{
            //    _userRepository.AddUserRole(user.Id);
            //    _logger.LogInformation("User created a new account with password.");

            //    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    //var callbackUrl = Url.Page(
            //    //    "/Account/ConfirmEmail",
            //    //    pageHandler: null,
            //    //    values: new { userId = user.Id, code = code },
            //    //    protocol: Request.Scheme);

            //    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            //    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return LocalRedirect(returnUrl);
            //}
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}

            // If we got this far, something failed, redisplay form
            return Page();
        }


    }
}