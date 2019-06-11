using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Admin.ViewComponents
{
    public class AdminLoginStatusViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminLoginStatusViewComponent(SignInManager<ApplicationUser> signInManager, IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (_signInManager.IsSignedIn(HttpContext.User) && user != null && _userRepository.IsAdmin(user))
            {
                return View("LoggedIn", user);
            }
            //if (_signInManager.IsSignedIn(HttpContext.User) && _userRepository.IsAdmin(user))
            //{
            //    return View("LoggedIn", user);
            //}
            return View();
        }
    }
}