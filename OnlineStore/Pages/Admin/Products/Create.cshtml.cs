using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.Products
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
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
        ViewData["EventId"] = new SelectList(_context.Event, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Item Item { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Item.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}