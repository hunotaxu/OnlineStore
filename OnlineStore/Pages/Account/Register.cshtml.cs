using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        [BindProperty]
        public new User User { get; set; }

        public RegisterModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.GetUserByUsername(User.Username);
                if (user != null)
                {
                    return Content("Tài khoản đã tồn tại");
                }
                _userRepository.Update(User);
                return RedirectToPage("/Home/Index");
            }
            return Page();
        }
    }
}