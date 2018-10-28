using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DAL.Repositories;

namespace OnlineStore.ViewComponents
{
    public class SearchResultViewComponent : ViewComponent
    {
        private readonly IItemRepository _itemRepository;

        public SearchResultViewComponent(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public IViewComponentResult InvokeAsync(string sTuKhoa)
        {
            //tìm kiếm theo ten sản phẩm
            var items = _itemRepository.GetItemByName(sTuKhoa);
            //ViewBag.TuKhoa = sTuKhoa;
            return View("Default",items.OrderBy(i => i.Price));
        }
    }
}
