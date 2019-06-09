using DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public interface IUserAddressRepository
    {
        UserAddress Find(Expression<Func<UserAddress, bool>> where);
        UserAddress GetByUserAndAddress(Guid userId, int addressId);
        IEnumerable<UserAddress> GetByUserId(Guid userId);
    }
}
