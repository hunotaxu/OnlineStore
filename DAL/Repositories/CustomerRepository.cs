using System.Linq;
using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class CustomerRepository : RepoBase<Customer>, ICustomerRepository
    {
        public bool CheckDuplicateCustomer(string email, string phoneNumber)
        {
            return Table.Any(c => c.Email == email || c.PhoneNumber == phoneNumber);
        }
    }
}
