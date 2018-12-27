using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public IEnumerable<CustomerCartViewModel> CustomerCartViewModel { get; set; }

        public void OnGet()
        {
            ////Lấy giỏ hàng 
            //GIOHANG lstGioHang = null;
            //List<ItemGioHang> lstItemGioHang = null;
            //SANPHAM sp = null;
            //List<KhachHang_GioHangViewModel> lstSP_KH = new List<KhachHang_GioHangViewModel>();
            //int maGioHang;
            //if (Session["NGUOIDUNG"] != null)
            //{
            //    NGUOIDUNG khachHang = Session["NGUOIDUNG"] as NGUOIDUNG;
            //    lstGioHang = LayGioHangKhachDaDangNhap(khachHang.MaNguoiDung);
            //    ViewBag.maGioHang = lstGioHang.MaGioHang;
            //    ViewBag.maKH = khachHang.MaNguoiDung;
            //    maGioHang = lstGioHang.MaGioHang;
            //    foreach (CHITIETGIOHANG ctgh in lstGioHang.CHITIETGIOHANGs)
            //    {
            //        sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == ctgh.MaSP);
            //        KhachHang_GioHangViewModel sp_KhachHang = new KhachHang_GioHangViewModel()
            //        {
            //            MaSP = ctgh.MaSP,
            //            TenSP = sp.TenSP,
            //            DonGia = sp.DonGia.Value,
            //            HinhAnh = sp.HinhAnh,
            //            SoLuong = ctgh.SoLuong.Value
            //        };
            //        lstSP_KH.Add(sp_KhachHang);
            //    }
            //}
            //else
            //{
            //    lstItemGioHang = LayGioHangKhachVangLai();
            //    ViewBag.maGioHang = null;
            //    ViewBag.maKH = null;
            //    foreach (ItemGioHang itemGioHang in lstItemGioHang)
            //    {
            //        KhachHang_GioHangViewModel sp_KhachHang = new KhachHang_GioHangViewModel()
            //        {
            //            MaSP = itemGioHang.MaSP,
            //            TenSP = itemGioHang.TenSP,
            //            DonGia = itemGioHang.DonGia,
            //            HinhAnh = itemGioHang.HinhAnh,
            //            SoLuong = itemGioHang.SoLuong

            //        };
            //        lstSP_KH.Add(sp_KhachHang);
            //    }
            //}
            //ViewBag.TongSoLuong = TinhTongSoLuong();
            //ViewBag.TongTien = TinhTongTien();
            //return View(lstSP_KH);
        }
    }
}