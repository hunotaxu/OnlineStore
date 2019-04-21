using System;
using System.Linq;
using System.Linq.Expressions;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Utilities.Commons;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DbSet<ApplicationUser> _userTable;
        private DbSet<ApplicationUserRole> _userRoletable;
        protected readonly OnlineStoreDbContext Db;

        public UserRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            
            Db = new OnlineStoreDbContext(options);
            _userRoletable = Db.Set<ApplicationUserRole>(); ;
            _userTable = Db.Set<ApplicationUser>();
        }

        public ApplicationUser FindUser(Expression<Func<ApplicationUser, bool>> where)
            => _userTable.FirstOrDefault(where);

        public bool IsDuplicatePhoneNumber(string phoneNumber)
        {
            return FindUser(u => string.Equals(u.PhoneNumber, phoneNumber)) != null;
        }

        public bool IsDuplicateEmail(string email)
        {
            return FindUser(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public int AddUserRole(Guid userId)
        {
            var userRole = new ApplicationUserRole
            {
                RoleId = new Guid(CommonConstants.CustomerId),
                UserId = userId,
                Status = Status.Active
            };
            _userRoletable.Add(userRole);
            return SaveChanges();
        }

        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}