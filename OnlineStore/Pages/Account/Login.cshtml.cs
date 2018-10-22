using System.Collections.Generic;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;

namespace OnlineStore.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDecentralizationRepository _userDecentralization;

        public LoginModel(IUserRepository userRepository, IUserDecentralizationRepository userDecentralization)
        {
            _userRepository = userRepository;
            _userDecentralization = userDecentralization;
        }

        public ActionResult OnPost(FormCollection f)
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            string username = f["txtUsername"].ToString();
            string password = f["txtPassword"].ToString();
            User user = _userRepository.GetUser(username, password);
            //NGUOIDUNG tv = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (user != null)
            {
                if (user.Status == 0)
                {
                    return Content("Tài khoản đã bị khóa!");
                }
                else
                {
                    IList<UserDecentralization> lstDecentralization = _userDecentralization.GetDecentralizations(user.TypeOfUserId);
                    string decentralization = "";
                    if (lstDecentralization != null)
                    {
                        foreach (var item in lstDecentralization)
                        {
                            decentralization += item.RoleId + ",";
                        }
                        decentralization = decentralization.Substring(0, decentralization.Length - 1);
                        //PhanQuyen(user.Id.ToString(), decentralization);
                        // Session["NGUOIDUNG"] = user;
                        HttpContext.Session.Set("User", user);
                        if (user.TypeOfUserId == 1)
                        {
                            return Content("<script>window.location.reload();</script>");
                        }
                        if (user.TypeOfUserId == 3)
                        {
                            return RedirectToPage("/Account/Index");
                            //return JavaScript("window.location = '" + Url.Action("Index", "QuanLyTaiKhoan") + "'");
                        }
                        if (user.TypeOfUserId == 4)
                        {
                            return RedirectToPage("/Account/Index");
                            //return JavaScript("window.location = '" + Url.Action("Index", "QuanLyTaiKhoan") + "'");
                        }
                        if (user.TypeOfUserId == 5)
                        {
                            return RedirectToPage("/Product/Index");
                            //return JavaScript("window.location = '" + Url.Action("Index", "QuanLySanPham") + "'");
                        }
                        if (user.TypeOfUserId == 6)
                        {
                            return RedirectToPage("/Customer/Index");
                            //return JavaScript("window.location = '" + Url.Action("Index", "QuanLyKhachHang") + "'");
                        }
                    }
                }
            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");
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