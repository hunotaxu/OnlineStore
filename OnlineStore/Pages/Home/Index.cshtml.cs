using System.Collections.Generic;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Home
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
            Phones = _itemRepository.GetByCategory(1);
            Laptops = _itemRepository.GetByCategory(2);
            Tablets = _itemRepository.GetByCategory(3);
        }
    }
}