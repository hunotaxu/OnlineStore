using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Areas.Admin.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public class InputModel
        {
            public int SourceId { get; set; }
            public int TargetId { get; set; }
            public List<string> Items { get; set; }
        }

        public IndexModel(ICategoryRepository categoryRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public IActionResult OnGetAll()
        {
            var categories = _categoryRepository.GetAll();
            IEnumerable<CategoryViewModel> model = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return new OkObjectResult(model);
        }

        public IActionResult OnGetById(int id)
        {
            CategoryViewModel model = _mapper.Map<CategoryViewModel>(_categoryRepository.Find(id));
            return new OkObjectResult(model);
        }

        public IActionResult OnPostSaveEntity([FromBody] DAL.Data.Entities.Category model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult("Đã xãy ra lỗi");
            }

            if (model.Id == 0)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                var maxSortOrderForInsert = _categoryRepository.GetSome(c => c.ParentId == null && c.IsDeleted == false).Max(x => x.SortOrder);
                if (model.ParentId != null)
                {
                    maxSortOrderForInsert = _categoryRepository.GetSome(c => c.ParentId == model.ParentId && c.IsDeleted == false).Max(x => x.SortOrder);
                }

                if (maxSortOrderForInsert.HasValue)
                {
                    model.SortOrder = (byte)(maxSortOrderForInsert.Value + 1);
                }
                else
                {
                    model.SortOrder = 1;
                }

                model.ParentId = model.ParentId;
                _categoryRepository.Add(model);
                return new OkObjectResult(model);
            }

            var category = _categoryRepository.Find(model.Id);
            category.Name = model.Name;
            category.ParentId = model.ParentId;
            var maxSortOrder = _categoryRepository.GetSome(c => c.ParentId == null && c.IsDeleted == false).Max(x => x.SortOrder);
            if (model.ParentId != null)
            {
                maxSortOrder = _categoryRepository.GetSome(c => c.ParentId == model.ParentId && c.IsDeleted == false).Max(x => x.SortOrder);
            }
            model.SortOrder = (byte)(maxSortOrder + 1);
            category.DateModified = DateTime.Now;
            _categoryRepository.Update(category);

            return new OkObjectResult(category);
        }

        public IActionResult OnPostDelete([FromBody] DAL.Data.Entities.Category model)
        {
            var category = _categoryRepository.Find(model.Id);
            _categoryRepository.Delete(category);
            var items = _itemRepository.GetSome(x => x.CategoryId == model.Id && x.IsDeleted == false);
            _itemRepository.DeleteRange(items);
            return new OkResult();
        }

        public IActionResult OnPostReOrder([FromBody] InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (input.SourceId == input.TargetId)
            {
                return new BadRequestResult();
            }
            var sourceCategory = _categoryRepository.Find(input.SourceId);
            var targetCategory = _categoryRepository.Find(input.TargetId);
            var tempOrder = sourceCategory.SortOrder;
            sourceCategory.SortOrder = targetCategory.SortOrder;
            _categoryRepository.Update(sourceCategory);
            targetCategory.SortOrder = tempOrder;
            _categoryRepository.Update(targetCategory);

            return new OkResult();
        }

        public IActionResult OnPostUpdateParentId([FromBody] InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            if (input.SourceId == input.TargetId)
            {
                return new BadRequestResult();
            }
            var sourceCategory = _categoryRepository.Find(input.SourceId);
            sourceCategory.ParentId = input.TargetId;
            _categoryRepository.Update(sourceCategory);
            for (byte i = 0; i < input.Items.Count; i++)
            {
                var child = _categoryRepository.Find(int.Parse(input.Items[i]));
                child.SortOrder = i;
                _categoryRepository.Update(child);
            }

            return new OkResult();
        }
    }
}