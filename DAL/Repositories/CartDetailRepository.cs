using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class CartDetailRepository : ICartDetailRepository
    {
        protected DbSet<CartDetail> Table;
        protected readonly OnlineStoreDbContext Db;

        public CartDetailRepository()
        {

        }
        public CartDetailRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<CartDetail>();
        }

        public CartDetail Find(Expression<Func<CartDetail, bool>> where)
            => Table.FirstOrDefault(where);

        public IEnumerable<CartDetail> GetItems (Expression<Func<CartDetail, bool>> where) => Table.Where(where);

        public int Add(CartDetail entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(CartDetail entity, bool persist = true)
        {
            Table.Update(entity);
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
