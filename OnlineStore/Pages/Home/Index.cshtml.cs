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

        //public IEnumerable<Item> _Phones { get; set; }
        //public IEnumerable<Item> _Laptops { get; set; }
        //public IEnumerable<Item> _Tablets { get; set; }
        //public IEnumerable<Item> _Accessories { get; set; }

        public List<Item> Phones { get; set; }
        public List<Item> Laptops { get; set; }
        public List<Item> Tablets { get; set; }
        public List<Item> Accessories { get; set; }


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
            Phones = new List<Item>();
            Laptops = new List<Item>();
            Tablets = new List<Item>();
            Accessories = new List<Item>();
        }
        public void OnGet()
        {
            var chuildPhoneCategory = _categoryRepository.GetSome(x=>x.IsDeleted == false && x.ParentId == 1);
            foreach (var item in chuildPhoneCategory)
            {               
                var phone = _itemRepository.GetByCategory(item.Id);
                foreach(var _phone in phone)
                {
                    Phones.Add(_phone);
                }
            }
            var chuildLaptopCategory = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == 2);
            foreach (var item in chuildLaptopCategory)
            {
                var laptop = _itemRepository.GetByCategory(item.Id);
                foreach (var _laptop in laptop)
                {
                    Laptops.Add(_laptop);
                }
            }
            var chuildTabletCategory = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == 3);
            foreach (var item in chuildTabletCategory)
            {
                var tablet = _itemRepository.GetByCategory(item.Id);
                foreach (var _tablet in tablet)
                {
                    Tablets.Add(_tablet);
                }
            }
            var chuildAccessorieCategory = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == 4);
            foreach (var item in chuildAccessorieCategory)
            {
                var accessorie = _itemRepository.GetByCategory(item.Id);
                foreach (var _accessorie in accessorie)
                {
                    Accessories.Add(_accessorie);
                }
            }
            var cus = _userManager.GetUserAsync(HttpContext.User).Result;
        }       
    }
}