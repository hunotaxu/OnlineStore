using System;
using System.Collections.Generic;
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
        private readonly DbSet<ApplicationUser> _userTable;
        private readonly DbSet<ApplicationUserRole> _userRoletable;
        protected readonly OnlineStoreDbContext Db;

        public UserRepository(DbContextOptions<OnlineStoreDbContext> options)
        {

            Db = new OnlineStoreDbContext(options);
            _userRoletable = Db.Set<ApplicationUserRole>(); ;
            _userTable = Db.Set<ApplicationUser>();
        }

        public ApplicationUser FindUser(Expression<Func<ApplicationUser, bool>> where)
            => _userTable.FirstOrDefault(where);

        public List<ApplicationUser> GetUsersByRole(Guid roleId)
        {
            var userIds = _userRoletable.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId);
            return _userTable.Where(u => userIds.Any(uid => uid.Equals(u.Id)) && u.Status == (byte)UserStatus.Active).ToList();
        }

        public List<ApplicationUser> GetEmployees()
        {
            var userIds = _userRoletable.Where(ur => ur.RoleId != CommonConstants.StoreOwnerRoleId && ur.RoleId != CommonConstants.CustomerRoleId).Select(ur => ur.UserId);
            return _userTable.Where(u => userIds.Contains(u.Id) && u.Status == (byte)UserStatus.Active).ToList();
        }

        public ApplicationUser FindUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;
            return FindUser(u =>
                string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Email, userName, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsDuplicatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;
            return FindUser(u => string.Equals(u.PhoneNumber, phoneNumber)) != null;
        }

        public bool IsDuplicateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            return FindUser(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public bool IsDuplicateEmail(string email, Guid userId)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userId.ToString()))
            {
                return false;
            }
            return FindUser(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase) && u.Id != userId) != null;
        }

        public bool IsProductManager(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return false;
            var user = FindUserByUserName(userName);
            return !_userRoletable.Where(u => u.UserId == user.Id).Any(x => x.RoleId != CommonConstants.ProductManagerRoleId);
        }

        public bool IsAdmin(ApplicationUser user)
        {
            if (user == null)
            {
                return false;
            }
            return !_userRoletable.Where(u => u.UserId == user.Id)
                .Any(a => a.RoleId == CommonConstants.CustomerRoleId);
        }

        public int AddUserRole(Guid userId, Guid roleId)
        {
            var userRole = new ApplicationUserRole
            {
                RoleId = roleId,
                UserId = userId,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Status = (byte)UserStatus.Active
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