using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        public PaginatedList<Item> Items { get; set; }

        public IndexModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IActionResult> OnGet(int? categoryId, int? pageIndex)
        {
            if (categoryId == null)
            {
                return BadRequest();
            }

            var items = _itemRepository.GetSome(item => item.CategoryId == categoryId);

            var enumerable = items as Item[] ?? items.ToArray();
            if (!enumerable.Any())
            {
                return NotFound();
            }

            //Tạo biến số sản phẩm trên trang
            int pageSize = 5;

            ViewData["CategoryId"] = categoryId;

            Items = await PaginatedList<Item>.CreateAsync(
                enumerable.OrderBy(n => n.Name), pageIndex ?? 1, pageSize);

            return Page();
        }

        public void OnPost()
        {

        }
    }
}