using System.Collections.Generic;
using AutoMapper;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly MapperConfiguration _mapperConfiguration;

        public IndexModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
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

        //public IActionResult OnGet()
        //{
        //    Items = _mapperConfiguration.CreateMapper().Map<IEnumerable<ItemViewModel>>(_itemRepository.GetAll(i => i.Category));
        //    return new OkObjectResult(Items);
        //}

        public IActionResult OnGetLoadAll()
        {
            var model = _itemRepository.GetAll(i => i.Category);
            Items = _mapperConfiguration.CreateMapper().Map<IEnumerable<ItemViewModel>>(model);
            return new OkObjectResult(Items);
        }

        //public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        //{
        //    var model=_itemRepository.
        //}
    }
}
