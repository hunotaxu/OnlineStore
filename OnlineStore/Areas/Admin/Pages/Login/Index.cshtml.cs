using System.Text.RegularExpressions;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;

namespace OnlineStore.Areas.Admin.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public IndexModel(ICustomerRepository customerRepository, IUserRepository userRepository)
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
            bool isEmail = Regex.IsMatch(username, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                ModelState.AddModelError(string.Empty, "Email không hợp lệ!");
                return Page();
            }
            User user = _userRepository.Find(x => x.Username.Equals(username));
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email hoặc số điện thoại không đúng!");
                return Page();
            }
            if (!user.Password.Equals(password))
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
                return Page();
            }

            HttpContext.Session.Set<User>("User", user);

            return RedirectToPage("/Admin/Users/Index");
        }
    }
}