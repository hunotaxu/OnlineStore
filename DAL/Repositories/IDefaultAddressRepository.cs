using DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public interface IDefaultAddressRepository
    {
        DefaultAddress Find(Expression<Func<DefaultAddress, bool>> where);
        IEnumerable<DefaultAddress> GetSome(Expression<Func<DefaultAddress, bool>> where);
        int Add(DefaultAddress entity, bool persist = true);
        int Update(DefaultAddress entity, bool persist = true);
        //int Update(int customerId, int addressId, int newQuantity, bool persist = true);
        int Delete(DefaultAddress entity, bool persist = true);
        int Delete(Guid customerId, int addressId, bool persist = true);
        int DeleteRange(IEnumerable<DefaultAddress> entities, bool persist = true);
        int DeleteRangeForever(IEnumerable<DefaultAddress> entities, bool persist = true);
        int SaveChanges();
    }
}