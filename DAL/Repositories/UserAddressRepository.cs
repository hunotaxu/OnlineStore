using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        public DbSet<UserAddress> Table;
        public readonly OnlineStoreDbContext Db;

        public UserAddressRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<UserAddress>();
        }

        public UserAddress Find(Expression<Func<UserAddress, bool>> where) => Table.FirstOrDefault(where);

        public UserAddress GetByUserAndAddress(Guid userId, int addressId)
        {
            return Table.FirstOrDefault(x => x.CustomerId == userId && x.AddressId == addressId);
        }
        public IEnumerable<UserAddress> GetByUserId(Guid userId)
        {
            return Table.Where(x => x.CustomerId == userId);
        }

        public int Add(UserAddress entity, bool persist = true)
        {
            Table.Add(entity);
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
