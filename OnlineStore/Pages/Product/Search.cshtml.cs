using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Product
{
    public class SearchModel : PageModel
    {
        private readonly IItemRepository _itemRepository;

        public SearchModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public IList<Item> Items { get; set; }

        public void OnGet()
        {

        }

        public void OnPost(string keyword)
        {
            ////tìm kiếm theo ten sản phẩm
            ////var lstSP = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa));

            //Items = _itemRepository.GetSome(i => i.Name.Contains((keyword))).ToList();

            //ViewData["Keyword"] = keyword;

            //if (!Items.Any())
            //{
            //    return RedirectToPage("/Home/Index");
            //}

            //string[] subItems = keyword.Split(' ');

            ////return View(lstSP.OrderBy(n => n.DonGia));
            //return PartialViewResult();
        }
    }
}