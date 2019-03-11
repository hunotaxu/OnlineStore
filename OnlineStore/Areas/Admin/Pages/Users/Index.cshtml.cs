using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;

namespace OnlineStore.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;

        public IndexModel(DAL.EF.OnlineStoreDbContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public string labelSearch { get; set; }
        public async Task OnGetAsync()
        {
            var Users = from m in _context.User
                         select m;

            User = await _context.User
                .Include(u => u.TypeOfUser).ToListAsync();
            if (!String.IsNullOrEmpty(SearchString))
            {
                Users = Users.Where(s => s.FirstName.Contains(SearchString) && s.Status == 1);
            }
            User = await Users.ToListAsync();

        }
    }
}
