using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class CustomerRepository : RepoBase<Customer>, ICustomerRepository
    {
    }
}
