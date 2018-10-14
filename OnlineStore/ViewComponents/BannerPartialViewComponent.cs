//using DAL.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using OnlineStore.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OnlineStore.ViewComponents
//{
//    public class BannerPartialViewComponent : ViewComponent
//    {
//        private readonly IItemRepository _itemRepository;

//        public BannerPartialViewComponent(IItemRepository itemRepository)
//        {
//            _itemRepository = itemRepository;
//        }

//        public IViewComponentResult Invoke()
//        {
//            //IEnumerable<SUKIEN> lstSuKien = db.SUKIENs.Where(n => n.NgayBatDau.Value.Date > DateTime.Now.Date);
//            //IEnumerable<SUKIEN> lstSuKien = db.SUKIENs.Where(n => n.NgayBatDau.Value.Date > DateTime.Now.Date);
//            //IEnumerable<Event> lstSuKien = db.SUKIENs;
//            //List<SUKIEN> lstSuKienCurrent = new List<SUKIEN>();
//            //int day = DateTime.Now.Day;
//            //int month = DateTime.Now.Month;
//            //int year = DateTime.Now.Year;
//            //foreach (SUKIEN sk1 in lstSuKien)
//            //{
//            //    if (DateTime.Compare(sk1.NgayBatDau.Value, DateTime.Now) < 0 && DateTime.Compare(sk1.NgayKetThuc.Value, DateTime.Now) > 0)
//            //    {
//            //        lstSuKienCurrent.Add(sk1);
//            //    }
//            //}
//            //foreach (SUKIEN sk1 in lstSuKien)
//            //{
//            //  if (sk1.NgayBatDau.Value.Year < year && sk1.NgayKetThuc.Value.Year > year)
//            //  {
//            //    if (sk1.NgayBatDau.Value.Month < month && sk1.NgayKetThuc.Value.Month > month)
//            //    {
//            //      lstSuKienCurrent.Add(sk1);
//            //    }
//            //    else if (sk1.NgayBatDau.Value.Month == month)
//            //    {
//            //      if (sk1.NgayBatDau.Value.Day < day && sk1.NgayKetThuc.Value.Day > day)
//            //      {
//            //        lstSuKienCurrent.Add(sk1);
//            //      }
//            //    }
//            //    else if (sk1.NgayKetThuc.Value.Month == month)
//            //    {
//            //      if (sk1.NgayBatDau.Value.Day < day && sk1.NgayKetThuc.Value.Day > day)
//            //      {
//            //        lstSuKienCurrent.Add(sk1);
//            //      }
//            //    }
//            //  }
//            //return PartialView(lstSuKienCurrent);
//        }
//    }
//}
