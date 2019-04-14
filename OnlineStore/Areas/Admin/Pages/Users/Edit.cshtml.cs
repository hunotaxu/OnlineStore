//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using DAL.Data.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using DAL.EF;
//using DAL.Models;
//using DAL.Repositories;

//namespace OnlineStore.Pages.Admin.Users
//{
//    public class EditModel : PageModel
//    {
//        private readonly DAL.EF.OnlineStoreDbContext _context;
//        //private IUserRepository _userRepository;

//        public EditModel(DAL.EF.OnlineStoreDbContext context, IUserRepository userRepository)
//        {
//            _context = context;
//            _userRepository = userRepository;
//        }

//        [BindProperty]
//        public User User { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            User = await _context.User
//                .Include(u => u.TypeOfUser).FirstOrDefaultAsync(m => m.Id == id);

//            if (User == null)
//            {
//                return NotFound();
//            }

//            ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUser, "Id", "Name");
//            return Page();

//        }

//        public IActionResult OnPostAsync(int Id)
//        {
//            //_context.Attach(User).State = EntityState.Modified;
//            User user = _userRepository.Find(Id);
//            user.Username = User.Username;
//            user.Status = User.Status;
//            try
//            {
//                _userRepository.Update(user);
//                //await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!UserExists(User.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return RedirectToPage("./Index");
//        }

//        private bool UserExists(int id)
//        {
//            return _context.User.Any(e => e.Id == id);
//        }
//    }
//}
