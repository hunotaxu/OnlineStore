using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.Suppliers
{
    public class IndexModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public IndexModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IList<Supplier> Supplier { get;set; }

        public async Task OnGetAsync()
        {
            Supplier = await _context.Supplier.ToListAsync();
        }
    }
}
