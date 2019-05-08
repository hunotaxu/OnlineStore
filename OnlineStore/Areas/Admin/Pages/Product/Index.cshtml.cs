using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using Utilities.DTOs;

namespace OnlineStore.Areas.Admin.Pages.Product
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
                cfg.CreateMap<DAL.Data.Entities.Category, CategoryViewModel>();
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

        public IActionResult OnGetById(int id)
        {
            var model = _mapperConfiguration.CreateMapper().Map<ItemViewModel>(_itemRepository.Find(id));
            return new OkObjectResult(model);
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

        public IActionResult OnPostSaveEntity([FromBody] Item model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (model.Id == 0)
            {
                model.DateCreated = DateTime.Now;
                //model.DateModified = DateTime.Now;
                _itemRepository.Add(model);
                return new OkObjectResult(model);
            }

            var item = _itemRepository.Find(model.Id);
            item.Name = model.Name;
            item.CategoryId = model.CategoryId;
            item.Description = model.Description;
            item.Price = model.Price;
            item.PromotionPrice = model.PromotionPrice;
            item.DateModified = DateTime.Now;
            _itemRepository.Update(item);

            return new OkObjectResult(item);
        }

        public IActionResult OnGetDelete(int id)
        {
            var item = _itemRepository.Find(id);
            _itemRepository.Delete(item);
            return new OkResult();
        }
    }
}