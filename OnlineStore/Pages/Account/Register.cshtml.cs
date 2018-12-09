using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        [BindProperty]
        public Customer Customer { get; set; }

        public RegisterModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_customerRepository.CheckDuplicateCustomer(Customer.Email, Customer.Password))
            {
                return Page();
            }
            Customer.TypeOfCustomerId = 1;
            _customerRepository.Add(Customer);
            _customerRepository.SaveChanges();

            return RedirectToPage("/Account/Login");
        }
    }
}