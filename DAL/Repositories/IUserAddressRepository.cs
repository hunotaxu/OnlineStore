using DAL.Data.Entities;
using DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public interface IUserAddressRepository
    {
        int Add(UserAddress entity, bool persist = true);
        int SaveChanges();
        UserAddress Find(Expression<Func<UserAddress, bool>> where);
        UserAddress GetByUserAndAddress(Guid userId, int addressId);
        IEnumerable<UserAddress> GetByUserId(Guid userId);
    }
}
