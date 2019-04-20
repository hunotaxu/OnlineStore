//using System.Linq;
//using DAL.Data.Entities;
//using DAL.EF;
//using DAL.Models;
//using DAL.Repositories.Base;
//using Microsoft.EntityFrameworkCore;

//namespace DAL.Repositories
//{
//    public class CustomerRepository : RepoBase<Customer>, ICustomerRepository
//    {
//        public bool CheckDuplicateCustomer(string email, string phoneNumber)
//        {
//            return Table.Any(c => c.Email == email || c.PhoneNumber == phoneNumber);
//        }

//        public CustomerRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
//        {
//        }
//    }
//}
