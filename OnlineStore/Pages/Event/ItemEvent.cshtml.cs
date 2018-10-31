using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Event
{
    public class ItemEventModel : PageModel
    {
        private readonly IItemRepository _itemRepository;

        public ItemEventModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public IActionResult OnGet(int? eventId, int? page)
        {
            if (eventId == null)
            {
                return BadRequest();
            }

            //var lstSP = db.SANPHAMs.Where(n => n.MaSuKien == maSuKien);

            IEnumerable<Item> items = _itemRepository.GetSome(n => n.EventId == eventId);

            if (!items.Any())
            {
                return NotFound();
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            //int PageSize = 5;
            ////Tạo biến thứ 2: Số trang hiện tại
            //int PageNumber = (page ?? 1);
            //ViewBag.MaSuKien = maSuKien;
            //return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
            return Page();
        }
    }
}