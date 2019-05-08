//using System.Threading.Tasks;
//using DAL.Data.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;

//namespace OnlineStore.Areas.Admin.Pages.Products
//{
//    public class DeleteModel : PageModel
//    {
//        private readonly DAL.EF.OnlineStoreDbContext _context;

//        public DeleteModel(DAL.EF.OnlineStoreDbContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public Item Item { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            Item = await _context.Item
//                .Include(i => i.Category)
//                .Include(i => i.Event).FirstOrDefaultAsync(m => m.Id == id);

//            if (Item == null)
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

//            Item = await _context.Item.FindAsync(id);

//            if (Item != null)
//            {
//                _context.Item.Remove(Item);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("./Index");
//        }
//    }
//}
