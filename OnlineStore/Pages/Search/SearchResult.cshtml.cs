using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;

namespace OnlineStore.Pages.Search
{
    public class SearchResultModel : PageModel
    {
        private readonly IItemRepository _itemRepository;

        public SearchResultModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public void OnGet(string sTuKhoa, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<Item> items = _itemRepository.GetItemByName(sTuKhoa);
            TempData["TuKhoa"] = sTuKhoa;
            //return Page(items.OrderBy(n => n.Name).PagedList(pageNumber, pageSize));
        }

        public void OnPost(string sTuKhoa)
        {
            int pageSize = 10;
            int pageNumber = 1;
            //tìm kiếm theo ten sản phẩm
            //var lstSP = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa));

            //ViewBag.TuKhoa = sTuKhoa;
            //return Page(lstSP.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
    }
}