using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;

namespace OnlineStore.Pages.Cart
{
    public class OrderModel : PageModel
    {
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;

        [BindProperty] public User User { get; set; }

        [BindProperty] public Order Order { get; set; }

        public void OnGet(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult OnPost(int? maKH, int? maGioHang, string diaChiNhanHang, string soDienThoaiNhanHang,
            string ho, string tenLot, string ten, string email)
        {
            if (!ModelState.IsValid)
            {
                DAL.Models.Cart cart = null;
                User user = HttpContext.Session.Get<User>("User");
                if (user != null)
                {
                    //NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
                    cart = _cartRepository.GetCartByCustomerId(user.Id);
                    // Thêm đơn đặt hàng ==> BindProperty cái model Order, tạo một view model dành riêng lưu thông tin người đặt hàng
                //    DONDATHANG ddh = new DONDATHANG
                //    {
                //        MaDDH = 1231,
                //        ThoiDiemDat = DateTime.Now,
                //        TinhTrangGiaoHang = -1,
                //        ThoiDiemLap = null,
                //        NgayGiaoDuKien = DateTime.Now.AddDays(3),
                //        UuDai = lstGioHang.UuDai,
                //        TongTien = TinhTongTien(),
                //        MaNV = null,
                //        MaKH = maKH,
                //        MaGioHang = maGioHang,
                //        PhiVanChuyen = 0,
                //        DiaChiNhanHang = diaChiNhanHang,
                //        SoDienThoaiNhanHang = soDienThoaiNhanHang
                //    };
                //    db.DONDATHANGs.Add(ddh);
                //    db.SaveChanges();
                //    TempData["result"] = "Đặt hàng thành công";
                //}
                //else
                //{
                //    NGUOIDUNG khachHangKhongCoTaiKhoan = new NGUOIDUNG()
                //    {
                //        MaLoaiNguoiDung = 2,
                //        Ho = ho,
                //        TenLot = tenLot,
                //        Ten = ten,
                //        DiaChi = diaChiNhanHang,
                //        SoDienThoai = soDienThoaiNhanHang,
                //        Email = email
                //    };
                //    db.NGUOIDUNGs.Add(khachHangKhongCoTaiKhoan);
                //    db.SaveChanges();
                //    GIOHANG gioHangCuaKhachVangLai = new GIOHANG()
                //    {
                //        MaKH = khachHangKhongCoTaiKhoan.MaNguoiDung,
                //        ThanhTien = TinhTongTien(),
                //        DaDat = true,
                //    };
                //    db.GIOHANGs.Add(gioHangCuaKhachVangLai);
                //    db.SaveChanges();
                //    DONDATHANG ddh = new DONDATHANG()
                //    {
                //        ThoiDiemDat = DateTime.Now,
                //        NgayGiaoDuKien = null,
                //        UuDai = 0,
                //        TinhTrangGiaoHang = -1,
                //        MaGioHang = gioHangCuaKhachVangLai.MaGioHang,
                //        MaKH = khachHangKhongCoTaiKhoan.MaNguoiDung,
                //        TongTien = TinhTongTien(),
                //        DiaChiNhanHang = diaChiNhanHang,
                //        SoDienThoaiNhanHang = soDienThoaiNhanHang
                //    };
                //    db.DONDATHANGs.Add(ddh);
                //    db.SaveChanges();
                //    Session["GioHang"] = null;
                //    TempData["result"] = "Đặt hàng thành công";
                }
                return RedirectToAction("XemGioHang");
            }
            return Page();
        }
    }
}