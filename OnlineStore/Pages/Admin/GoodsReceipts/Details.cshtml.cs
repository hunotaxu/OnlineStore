using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public DetailsModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public GoodsReceipt GoodsReceipt { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GoodsReceipt = await _context.GoodsReceipt
                .Include(g => g.Supplier).FirstOrDefaultAsync(m => m.Id == id);

            if (GoodsReceipt == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
