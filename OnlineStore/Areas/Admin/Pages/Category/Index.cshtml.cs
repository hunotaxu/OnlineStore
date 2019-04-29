using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Areas.Admin.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly MapperConfiguration _mapperConfiguration;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<DAL.Data.Entities.Category, CategoryViewModel>();
            });
        }

        public IActionResult OnGetAll()
        {
            var categories = _categoryRepository.GetAll();
            var model = _mapperConfiguration.CreateMapper().Map<IEnumerable<CategoryViewModel>>(categories);
            return new OkObjectResult(model);
        }
    }
}