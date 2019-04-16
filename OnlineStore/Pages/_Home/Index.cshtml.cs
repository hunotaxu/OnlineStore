﻿using System.Collections.Generic;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages._Home
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;

        public IEnumerable<Item> Phones { get; set; }
        public IEnumerable<Item> Laptops { get; set; }
        public IEnumerable<Item> Tablets { get; set; }

        public IndexModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public void OnGet()
        {
            //Lần lượt tạo các viewbag để lấy list sản phẩm từ cơ sở dữ liệu
            //List Diện điện thoại mới nhất
            //var lstDTM = db.SANPHAMs.Where(n => n.MaLoaiSP == 1/* && n.Moi == 1*/ && n.DaXoa == false);
            Phones = _itemRepository.GetByCategory(1);
            //Gán vào ViewBag
            //ViewBag.ListDTM = lstDTM;

            //List LapTop mới nhất
            //var lstLT = db.SANPHAMs.Where(n => n.MaLoaiSP == 2 /*&& n.Moi == 1 */&& n.DaXoa == false);
            //IEnumerable<SANPHAM> lstSPLT = db.SANPHAMs.Where(n => n.MaLoaiSP == 2 && n.DaXoa == false);
            Laptops = _itemRepository.GetByCategory(2);
            //Gán vào ViewBag
            //ViewBag.ListLTM = lstLT;

            //List Máy tính bảng mới nhất
            //var lstMTB = db.SANPHAMs.Where(n => n.MaLoaiSP == 3 /*&& n.Moi == 1*/ && n.DaXoa == false);
            Tablets = _itemRepository.GetByCategory(3);
            //Gán vào ViewBag
            //ViewBag.ListMTBM = lstMTB;
        }
    }
}