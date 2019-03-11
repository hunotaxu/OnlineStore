using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts.GoodsReceiptDetails
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public DeleteModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GoodsReceiptDetail GoodsReceiptDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GoodsReceiptDetail = await _context.GoodsReceiptDetail
                .Include(g => g.GoodsReceipt)
                .Include(g => g.Item).FirstOrDefaultAsync(m => m.ItemId == id);

            if (GoodsReceiptDetail == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GoodsReceiptDetail = await _context.GoodsReceiptDetail.FindAsync(id);

            if (GoodsReceiptDetail != null)
            {
                _context.GoodsReceiptDetail.Remove(GoodsReceiptDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
