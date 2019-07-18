using System.Collections.Generic;
using DAL.Data.Entities;

namespace OnlineStore.Models.ViewModels
{
    public class MenuCategoryViewModel
    {
        public IEnumerable<Category> ParentCategories { get; set; }
        public IEnumerable<Category> ChildCategories { get; set; }
    }
}