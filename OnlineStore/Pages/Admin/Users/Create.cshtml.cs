using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.Users
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
            ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUser, "Id", "Name");
            ViewData["Gender"] = new SelectList(_context.User, "Gender", "");
            return Page();
            
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Status = 1;
            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}