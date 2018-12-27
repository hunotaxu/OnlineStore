using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts.GoodsReceiptDetails
{
    public class CreateModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public CreateModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["GoodsReceiptId"] = new SelectList(_context.GoodsReceipt, "Id", "Id");
        ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public GoodsReceiptDetail GoodsReceiptDetail { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.GoodsReceiptDetail.Add(GoodsReceiptDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}