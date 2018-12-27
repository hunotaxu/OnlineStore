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
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

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

        public async Task OnPostAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            //NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            List<Item> itemResult = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                itemResult = _itemRepository.GetSome(i => i.Name.Contains((searchString))).ToList();
            }

            switch (sortOrder)
            {
                case "lowtohigh":
                    itemResult = new List<Item>(itemResult.OrderBy(s => s.Price));
                    break;
                case "hightolow":
                    itemResult = new List<Item>(itemResult.OrderByDescending(s => s.Price));
                    break;
                default:
                    itemResult = new List<Item>(itemResult.OrderByDescending(s => s.View));
                    break;
            }

            int pageSize = 5;
            Items = await PaginatedList<Item>.CreateAsync(itemResult, pageIndex ?? 1, pageSize);
        }
    }
}