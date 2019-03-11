using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public interface ICustomerRepository : IRepo<Customer>
    {
        bool CheckDuplicateCustomer(string email, string phoneNumber);
    }
}
