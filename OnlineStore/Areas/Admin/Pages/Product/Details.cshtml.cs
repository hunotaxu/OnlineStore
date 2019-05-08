//using System.Threading.Tasks;
//using DAL.Data.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;

//namespace OnlineStore.Areas.Admin.Pages.Products
//{
//    public class DetailsModel : PageModel
//    {
//        private readonly DAL.EF.OnlineStoreDbContext _context;

//        public DetailsModel(DAL.EF.OnlineStoreDbContext context)
//        {
//            _context = context;
//        }

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
//    }
//}
