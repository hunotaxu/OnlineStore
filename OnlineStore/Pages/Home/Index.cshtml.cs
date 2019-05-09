using System.Collections.Generic;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public IEnumerable<Item> Phones { get; set; }
        public IEnumerable<Item> Laptops { get; set; }
        public IEnumerable<Item> Tablets { get; set; }
        public IEnumerable<Item> Accessories { get; set; }


        public IndexModel(IItemRepository itemRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _itemRepository = itemRepository;
        }

        public void OnGet()
        {
            var cus = _userManager.GetUserAsync(HttpContext.User).Result;
            Phones = _itemRepository.GetByCategory(1);
            Laptops = _itemRepository.GetByCategory(2);
            Tablets = _itemRepository.GetByCategory(3);
            Accessories = _itemRepository.GetByCategory(4);
        }
    }
}