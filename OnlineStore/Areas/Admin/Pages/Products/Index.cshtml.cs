using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public IndexModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; }

        public async Task OnGetAsync()
        {
            Item = await _context.Item
                .Include(i => i.Category)
                .Include(i => i.Event).ToListAsync();
        }
    }
}
