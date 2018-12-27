using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts
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
        ViewData["SupplierId"] = new SelectList(_context.Supplier, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public GoodsReceipt GoodsReceipt { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.GoodsReceipt.Add(GoodsReceipt);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}