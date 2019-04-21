using System;
using System.Linq.Expressions;
using DAL.Data.Entities;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser FindUser(Expression<Func<ApplicationUser, bool>> where);

        bool IsDuplicatePhoneNumber(string phoneNumber);

        bool IsDuplicateEmail(string email);

        int AddUserRole(Guid userId);

        int SaveChanges();
    }
}
