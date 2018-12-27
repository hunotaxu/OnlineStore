using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Admin.Charts
{
    public class BarChartModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public BarChartModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; }
        public IList<LineItem> LineItems { get; set; }
        public async Task OnGetAsync()
        {
            //Order = await _context.Order
            //    .Include(o => o.Customer).ToListAsync();
        }

        public ActionResult BarChart()
        {
            return Page();
        }
        


    }
}
