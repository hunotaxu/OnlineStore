using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using DAL.Data.Entities;

namespace OnlineStore.Areas.Admin.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly MapperConfiguration _mapperConfiguration;
        public class InputModel
        {
            public int sourceId { get; set; }
            public int targetId { get; set; }
            public List<string> items { get; set; }
        }
        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<DAL.Data.Entities.Category, CategoryViewModel>();
                config.CreateMap<CategoryViewModel, DAL.Data.Entities.Category>();
            });
        }

        public IActionResult OnGetAll()
        {
            var categories = _categoryRepository.GetAll();
            var model = _mapperConfiguration.CreateMapper().Map<IEnumerable<CategoryViewModel>>(categories);
            return new OkObjectResult(model);
        }

        public IActionResult OnGetById(int id)
        {
            var model = _mapperConfiguration.CreateMapper().Map<CategoryViewModel>(_categoryRepository.Find(id));
            return new OkObjectResult(model);
        }

        public IActionResult OnPostSaveEntity([FromBody] DAL.Data.Entities.Category category)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (category.Id == 0)
            {
                _categoryRepository.Add(category);
            }
            else
            {
                _categoryRepository.Update(category);
            }

            return new OkObjectResult(category);
        }

        public IActionResult OnPostDeleteCategory(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var category = _categoryRepository.Find(categoryId);
            _categoryRepository.Delete(category);
            return new OkResult();
        }

        public IActionResult OnPostReOrder([FromBody]InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (input.sourceId == input.targetId)
            {
                return new BadRequestResult();
            }
            var sourceCategory = _categoryRepository.Find(input.sourceId);
            var targetCategory = _categoryRepository.Find(input.targetId);
            var tempOrder = sourceCategory.SortOrder;
            sourceCategory.ParentId = targetCategory.ParentId;
            sourceCategory.SortOrder = targetCategory.SortOrder;
            _categoryRepository.Update(sourceCategory);
            targetCategory.SortOrder = tempOrder;
            _categoryRepository.Update(targetCategory);

            return new OkResult();
        }

        public IActionResult OnPostUpdateParentId([FromBody]InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (input.sourceId == input.targetId)
            {
                return new BadRequestResult();
            }
            var sourceCategory = _categoryRepository.Find(input.sourceId);
            sourceCategory.ParentId = input.targetId;
            _categoryRepository.Update(sourceCategory);
            for (var i = 0; i < input.items.Count; i++)
            {
                var child = _categoryRepository.Find(int.Parse(input.items[i]));
                child.SortOrder = i;
                _categoryRepository.Update(child);
            }

            return new OkResult();
        }
    }
}