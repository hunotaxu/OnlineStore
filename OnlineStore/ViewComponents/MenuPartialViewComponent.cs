using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Entities;
using System.Linq;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.ViewComponents
{
    public class MenuPartialViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public MenuPartialViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            //Truy vấn lấy về 1 list các sản phẩm
            IEnumerable<Category> categories = _categoryRepository.GetSome(x => x.IsDeleted == false);
            //return Task.FromResult<IViewComponentResult>(View("Default", categories));
            var menus = new MenuCategoryViewModel
            {
                ParentCategories = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId == null).OrderBy(y => y.SortOrder),
                ChildCategories = _categoryRepository.GetSome(x => x.IsDeleted == false && x.ParentId != null).OrderBy(y => y.SortOrder)
            };
            return Task.FromResult<IViewComponentResult>(View("Default", menus));
        }
    }
}