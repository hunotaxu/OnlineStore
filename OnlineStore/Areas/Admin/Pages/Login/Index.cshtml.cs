using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineStore.Models.ViewModels;
using Utilities.DTOs;

namespace OnlineStore.Areas.Admin.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        public IndexModel(ICustomerRepository customerRepository, IUserRepository userRepository, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<IndexModel> logger)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return new OkObjectResult(new GenericResult(true));
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new ObjectResult(new GenericResult(false, "Tài khoản đã bị khóa"));
                }
                else
                {
                    return new ObjectResult(new GenericResult(false, "Đăng nhập sai"));
                }
            }

            // If we got this far, something failed, redisplay form
            return new ObjectResult(new GenericResult(true, model));
        }

        //public IActionResult OnPost(string username, string password, string url, string itemId)
        //{
        //    bool isEmail = Regex.IsMatch(username, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        //    if (!isEmail)
        //    {
        //        ModelState.AddModelError(string.Empty, "Email không hợp lệ!");
        //        return Page();
        //    }
        //    User user = _userRepository.Find(x => x.Username.Equals(username));
        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Email hoặc số điện thoại không đúng!");
        //        return Page();
        //    }
        //    if (!user.Password.Equals(password))
        //    {
        //        ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
        //        return Page();
        //    }

        //    HttpContext.Session.Set<User>("User", user);

        //    return RedirectToPage("/Admin/Users/Index");
        //}
    }
}