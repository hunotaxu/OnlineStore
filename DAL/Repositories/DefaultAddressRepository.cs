using DAL.Data.Entities;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class DefaultAddressRepository : IDefaultAddressRepository
    {
        protected DbSet<DefaultAddress> Table;
        protected readonly OnlineStoreDbContext Db;

        public DefaultAddressRepository()
        {

        }
        public DefaultAddressRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<DefaultAddress>();
        }

        public DefaultAddress Find(Expression<Func<DefaultAddress, bool>> where)
            => Table.FirstOrDefault(where);

        public IEnumerable<DefaultAddress> GetSome(Expression<Func<DefaultAddress, bool>> where) => Table.Where(where);

        public int Add(DefaultAddress entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(DefaultAddress entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        //public int Update(Guid customerId, int cartId, int newQuantity, bool persist = true)
        //{
        //    DefaultAddress cartDetail = Find(c => c.AddressId == cartId && c.CustomerId == customerId);
        //    Table.Attach(cartDetail).State = EntityState.Deleted;
        //    return persist ? SaveChanges() : 0;
        //}

        public virtual int Delete(DefaultAddress entity, bool persist = true)
        {
            entity.IsDeleted = true;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(Guid customerId, int addressId, bool persist = true)
        {
            DefaultAddress cartDetail = Find(c => c.AddressId == addressId && c.CustomerId == customerId);
            cartDetail.IsDeleted = true;
            Table.Update(cartDetail);
            //Table.Attach(cartDetail).State = EntityState.IsDeleted;
            return persist ? SaveChanges() : 0;
        }

        public int DeleteRange(IEnumerable<DefaultAddress> entities, bool persist = true)
        {
            foreach (DefaultAddress cartDetail in entities)
            {
                cartDetail.IsDeleted = true;
                Table.Update(cartDetail);
            }
            //Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
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
