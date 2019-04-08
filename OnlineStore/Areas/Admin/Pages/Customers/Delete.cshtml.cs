//using System.Threading.Tasks;
//using DAL.Data.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;

//namespace OnlineStore.Areas.Admin.Pages.Customers
//{
//    public class DeleteModel : PageModel
//    {
//        private readonly DAL.EF.OnlineStoreDbContext _context;

//        public DeleteModel(DAL.EF.OnlineStoreDbContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public Customer Customer { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            Customer = await _context.Customer
//                .Include(c => c.TypeOfCustomer).FirstOrDefaultAsync(m => m.Id == id);

//            if (Customer == null)
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

//            Customer = await _context.Customer.FindAsync(id);

//            if (Customer != null)
//            {
//                _context.Customer.Remove(Customer);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("./Index");
//        }
//    }
//}
