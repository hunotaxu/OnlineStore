using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.GoodsReceipts.GoodsReceiptDetails
{
    public class IndexModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public IndexModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IList<GoodsReceiptDetail> GoodsReceiptDetail { get;set; }

        //public async Task OnGetAsync()
        //{
        //    GoodsReceiptDetail = await _context.GoodsReceiptDetail
        //        .Include(g => g.GoodsReceipt)
        //        .Include(g => g.Item).ToListAsync();
        //}
        

        public GoodsReceipt GoodsReceipt { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
           
         
                GoodsReceiptDetail = await _context.GoodsReceiptDetail
                    .Include(g => g.GoodsReceipt)
                    .Include(g => g.Item).ToListAsync();
           
            //GoodsReceipt = await _context.GoodsReceipt
            //    .Include(g => g.Supplier).FirstOrDefaultAsync(m => m.Id == id);

            if (GoodsReceipt == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
