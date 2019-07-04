using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly ICategoryRepository _categoryRepository;
        public int _numbercartitem;

        public IEnumerable<Item> Phones { get; set; }
        public IEnumerable<Item> Laptops { get; set; }
        public IEnumerable<Item> Tablets { get; set; }
        public IEnumerable<Item> Accessories { get; set; }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }

        public IndexModel(ICategoryRepository categoryRepository,ICartRepository cartRepository, IUserRepository userRepository, ICartDetailRepository cartDetailRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _categoryRepository = categoryRepository;

        }


        public void OnGet()
        {

            var chuildPhoneCategory = _categoryRepository.GetSome(x=>x.IsDeleted == false && x.ParentId == 1);
            foreach (var item in chuildPhoneCategory)
            {               
                    Phones = _itemRepository.GetByCategory(item.Id);                
            }
            var chuildLaptopCategory = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == 2);
            foreach (var item in chuildLaptopCategory)
            {
                Laptops = _itemRepository.GetByCategory(item.Id);
            }
            var chuildTabletCategory = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == 3);
            foreach (var item in chuildTabletCategory)
            {
                Tablets = _itemRepository.GetByCategory(item.Id);
            }
            var chuildAccessorieCategory = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == 4);
            foreach (var item in chuildAccessorieCategory)
            {
                Accessories = _itemRepository.GetByCategory(item.Id);
            }
           
            var cus = _userManager.GetUserAsync(HttpContext.User).Result;
        }       
    }
}