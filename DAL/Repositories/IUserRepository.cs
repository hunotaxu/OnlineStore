using System;
using System.Linq.Expressions;
using DAL.Data.Entities;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser Find(Expression<Func<ApplicationUser, bool>> where);

        bool IsDuplicatePhoneNumber(string phoneNumber);

        bool IsDuplicateEmail(string email);
    }
}
