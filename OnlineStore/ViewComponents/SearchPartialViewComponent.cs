using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace OnlineStore.ViewComponents
{
    public class SearchPartialViewComponent: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            
            return Task.FromResult<IViewComponentResult>(View("Default"));
        }
    }
}
