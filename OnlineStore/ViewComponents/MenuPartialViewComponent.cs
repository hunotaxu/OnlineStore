using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.ViewComponents
{
    public class MenuPartialViewComponent : ViewComponent
    {
        //private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public MenuPartialViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            //_itemRepository = itemRepository;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            //Truy vấn lấy về 1 list các sản phẩm
            //var items = _itemRepository.GetAll();
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            //var lstSP = _itemRepository. db.SANPHAMs;
            //return View("Default", lstSP);
            return Task.FromResult<IViewComponentResult>(View("Menubar", categories));
        }
    }
}
