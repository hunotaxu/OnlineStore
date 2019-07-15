using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Utilities.Commons;

namespace OnlineStore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
            [DataType(DataType.Text)]
            [Display(Name = "Họ tên")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
            [Display(Name = "Ngày sinh")]
            [DataType(DataType.Date, ErrorMessage = "Định dạng ngày chưa đúng.")]
            public DateTime DOB { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập email.")]
            [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [StringLength(32, ErrorMessage = "Vui lòng nhập {0} có tổi thiểu {2} ký tự và tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
            [Phone(ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
            [Display(Name = "Giới tính")]
            public byte Gender { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            returnUrl = returnUrl ?? CommonConstants.UrlHomePage;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_userRepository.IsDuplicateEmail(Input.Email) || _userRepository.IsDuplicatePhoneNumber(Input.PhoneNumber))
            {
                ModelState.AddModelError(string.Empty, CommonConstants.ErrorDuplicatePhoneNumer);
                return Page();
            }

            var user = new ApplicationUser
            {
                Name = Input.Name,
                DOB = Input.DOB,
                UserName = Input.PhoneNumber,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Gender = (byte)Input.Gender,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _userRepository.AddUserRole(user.Id, CommonConstants.CustomerRoleId);
                _logger.LogInformation("User created a new account with password.");

                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { userId = user.Id, code = code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
