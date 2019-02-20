using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts
{
    public class EditModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public EditModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(GoodsReceipt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoodsReceiptExists(GoodsReceipt.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GoodsReceiptExists(int id)
        {
            return _context.GoodsReceipt.Any(e => e.Id == id);
        }
    }
}
