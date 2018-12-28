using System.Linq;
using System.Threading.Tasks;
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

        public Task<IViewComponentResult> InvokeAsync(string sTuKhoa)
        {
            var items = _itemRepository.GetItemByName(sTuKhoa);
            return Task.FromResult<IViewComponentResult>(View("Default", items.OrderBy(i => i.Price)));
        }
    }
}
