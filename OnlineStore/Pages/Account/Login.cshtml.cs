using System.ComponentModel;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        //public string UsernameMessage { get; set; }

        //public string PasswordMessage { get; set; }

        public LoginModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            //UsernameMessage = "";
            //PasswordMessage = "";
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost(string username, string password)
        {
            Customer cus = _customerRepository.Find(c => c.Email.Equals(username) || c.PhoneNumber.Equals(username));
            if (cus == null)
            {
                ModelState.AddModelError(string.Empty, "Email hoặc số điện thoại không đúng!");
                return Page();
            }

            if (!cus.Password.Equals(password))
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
                return Page();
            }

            HttpContext.Session.Set<Customer>("Customer", cus);

            return RedirectToPage("/Home/Index");
        }

        //[Authorize(Roles = "31")]
        //public void PhanQuyen(string tv, string Quyen)
        //{
        //    FormsAuthentication.Initialize();
        //    var ticket = new FormsAuthenticationTicket(1, tv, DateTime.Now, DateTime.Now.AddHours(3), true, Quyen);
        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));//value (đã được mã hóa)
        //    if (ticket.IsPersistent)//true nếu cookie đã được cấp 
        //    {
        //        cookie.Expires = ticket.Expiration;//cấp thời gian sống cho cookie
        //    }
        //    Response.Cookies.Add(cookie);//gán cookie trả về cho client
        //}
    }
}