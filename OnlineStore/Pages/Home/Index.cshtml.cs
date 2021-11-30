using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using System.Collections.Generic;

namespace OnlineStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        public int _numbercartitem;

        public List<Item> Phones { get; set; }
        public List<Item> Laptops { get; set; }
        public List<Item> Tablets { get; set; }
        public List<Item> Accessories { get; set; }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }

        public IndexModel(ICategoryRepository categoryRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            Phones = new List<Item>();
            Laptops = new List<Item>();
            Tablets = new List<Item>();
            Accessories = new List<Item>();
        }

        public void OnGet()
        {
            foreach (Category item in _categoryRepository.GetSome(x => !x.IsDeleted && x.ParentId == 1))
            {
                foreach (Item _phone in _itemRepository.GetByCategory(item.Id))
                {
                    Phones.Add(_phone);
                }
            }
            foreach (Category item in _categoryRepository.GetSome(x => !x.IsDeleted && x.ParentId == 2))
            {
                foreach (Item _laptop in _itemRepository.GetByCategory(item.Id))
                {
                    Laptops.Add(_laptop);
                }
            }
            foreach (Category item in _categoryRepository.GetSome(x => !x.IsDeleted && x.ParentId == 3))
            {
                foreach (Item _tablet in _itemRepository.GetByCategory(item.Id))
                {
                    Tablets.Add(_tablet);
                }
            }
            foreach (Category item in _categoryRepository.GetSome(x => !x.IsDeleted && x.ParentId == 4))
            {
                foreach (Item _accessorie in _itemRepository.GetByCategory(item.Id))
                {
                    Accessories.Add(_accessorie);
                }
            }
            ApplicationUser cus = _userManager.GetUserAsync(HttpContext.User).Result;
        }
    }
}