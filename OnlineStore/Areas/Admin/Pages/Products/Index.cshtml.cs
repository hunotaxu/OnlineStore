using System.Collections.Generic;
using AutoMapper;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using Utilities.DTOs;

namespace OnlineStore.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly MapperConfiguration _mapperConfiguration;

        public IndexModel(IItemRepository itemRepository, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Item, ItemViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();
            });
        }

        public IEnumerable<ItemViewModel> Items { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnGetAll()
        {
            var model = _itemRepository.GetAll(i => i.Category);
            Items = _mapperConfiguration.CreateMapper().Map<IEnumerable<ItemViewModel>>(model);
            return new OkObjectResult(Items);
        }

        public IActionResult OnGetAllCategories()
        {
            var categories = _mapperConfiguration.CreateMapper()
                .Map<IEnumerable<CategoryViewModel>>(_categoryRepository.GetAll());
            return new OkObjectResult(categories);
        }

        public IActionResult OnGetAllPaging(int? categoryId, string keyword, int pageIndex, int pageSize)
        {
            //var admin = HttpContext.Session.Get<ApplicationUser>(CommonConstants.UserSession);
            //if (admin == null || !_userRepository.IsProductManager(admin.UserName))
            //{
            //    return new JsonResult(new { authenticate = false });
            //}
            var model = _itemRepository.GetAllPaging(categoryId, keyword, pageIndex, pageSize);
            var itemsPagination = _mapperConfiguration.CreateMapper().Map<PagedResult<ItemViewModel>>(model);
            return new OkObjectResult(itemsPagination);
        }
    }
}