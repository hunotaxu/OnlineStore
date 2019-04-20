using System;
using System.Linq;
using System.Linq.Expressions;
using DAL.Data.Entities;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DbSet<ApplicationUser> _table;
        protected readonly OnlineStoreDbContext Db;

        public UserRepository()
        {

        }

        public UserRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            _table = Db.Set<ApplicationUser>();
        }

        public ApplicationUser Find(Expression<Func<ApplicationUser, bool>> where)
            => _table.FirstOrDefault(where);

        public bool IsDuplicatePhoneNumber(string phoneNumber)
        {
            return Find(u => string.Equals(u.PhoneNumber, phoneNumber)) != null;
        }

        public bool IsDuplicateEmail(string email)
        {
            return Find(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)) != null;
        }
    }
}