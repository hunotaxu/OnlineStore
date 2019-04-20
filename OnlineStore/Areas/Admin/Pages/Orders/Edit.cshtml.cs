using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;
using DAL.Repositories;

namespace OnlineStore.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;
        private IOrderRepository _orderRepository;
        private IAddressRepository _addressRepository;

        public EditModel(DAL.EF.OnlineStoreDbContext context, IOrderRepository orderRepository, IAddressRepository addressRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
        }

        [BindProperty]
        public DAL.Data.Entities.Order Order { get; set; }

        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Order = await _context.Order
            //    .Include(o => o.Customer).FirstOrDefaultAsync(m => m.Id == id);

            Order = _orderRepository.Find(id);
            Address = _addressRepository.Find(Order.AddressId);
            if (Order == null)
            {
                return NotFound();
            }
           //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "Id", "Password");
            return Page();
        }

        public IActionResult OnPostAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order = _orderRepository.Find(Id);
            _context.Attach(Order).State = EntityState.Modified;
            Order.Status = StatusOrder.ReadyToShip;
            try
            {
                _orderRepository.Update(Order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
