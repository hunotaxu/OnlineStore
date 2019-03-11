using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts.GoodsReceiptDetails
{
    public class EditModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public EditModel(DAL.EF.OnlineStoreDbContext context)
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
           ViewData["GoodsReceiptId"] = new SelectList(_context.GoodsReceipt, "Id", "Id");
           ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(GoodsReceiptDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoodsReceiptDetailExists(GoodsReceiptDetail.ItemId))
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

        private bool GoodsReceiptDetailExists(int id)
        {
            return _context.GoodsReceiptDetail.Any(e => e.ItemId == id);
        }
    }
}
