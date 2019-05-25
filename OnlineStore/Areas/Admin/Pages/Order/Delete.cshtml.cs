//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using DAL.EF;
//using DAL.Models;

//namespace OnlineStore.Pages.Admin.Orders
//{
//    public class DeleteModel : PageModel
//    {
//        private readonly DAL.EF.OnlineStoreDbContext _context;

//        public DeleteModel(DAL.EF.OnlineStoreDbContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public DAL.Data.Entities.Order Order { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            Order = await _context.Order
//                .Include(o => o.Customer).FirstOrDefaultAsync(m => m.Id == id);

//            if (Order == null)
//            {
//                return NotFound();
//            }
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            Order = await _context.Order.FindAsync(id);

//            if (Order != null)
//            {
//                _context.Order.Remove(Order);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("./Index");
//        }
//    }
//}
