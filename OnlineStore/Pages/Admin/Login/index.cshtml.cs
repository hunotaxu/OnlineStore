using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Pages.LoginPage
{
    public class indexModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        //public string UsernameMessage { get; set; }

        //public string PasswordMessage { get; set; }

        public indexModel(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost(string username, string password, string url, string itemId)
        {

            User user_ = _userRepository.Find(x => x.Username.Equals(username));

            if (user_ == null)
            {
                ModelState.AddModelError(string.Empty, "Email hoặc số điện thoại không đúng!");
                return Page();
            }
            if (!user_.Password.Equals(password))
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
                return Page();
            }

            HttpContext.Session.Set<User>("User", user_);

            return RedirectToPage("/Admin/Users/Index");
        }
    }
}