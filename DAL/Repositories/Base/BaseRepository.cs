using DAL.Data.Entities.Base;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories.Base
{
    public class BaseRepository<T> : IRepo<T> where T : EntityBase
    {
        protected readonly OnlineStoreDbContext Db;

        public BaseRepository(DbContextOptions<OnlineStoreDbContext> options, OnlineStoreDbContext context = null)
        {
            Db = context ?? new OnlineStoreDbContext(options);
            Table = Db.Set<T>();
        }

        protected DbSet<T> Table;
        public OnlineStoreDbContext Context => Db;

        public bool HasChanges => Db.ChangeTracker.HasChanges();

        public int Count => Table.Count();

        //public string GetTableName()
        //{
        //    var metaData = Db.Model.FindEntityType(typeof(T).FullName).SqlServer();
        //    return $"{metaData.Schema}.{metaData.TableName}";
        //}

        public T Find(int? id) => Table.Find(id);

        public T Find(Expression<Func<T, bool>> where)
            => Table.FirstOrDefault(where);

        public T Find<TIncludeField>(Expression<Func<T, bool>> where, Expression<Func<T, ICollection<TIncludeField>>> include)
            => Table.Where(where).Include(include).FirstOrDefault();

        public T Find<TIncludeField>(Expression<Func<T, bool>> where, Expression<Func<T, TIncludeField>> include)
            => Table.Where(where).Include(include).FirstOrDefault();

        public virtual IEnumerable<T> GetAll() => Table.Where(x => x.IsDeleted == false);

        public IEnumerable<T> GetAll<TIncludeField>(Expression<Func<T, ICollection<TIncludeField>>> include)
            => Table.Include(include);

        public IEnumerable<T> GetAll<TIncludeField>(Expression<Func<T, TIncludeField>> include)
            => Table.Include(include);

        public IEnumerable<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending)
            => ascending ? Table.OrderBy(orderBy) : Table.OrderByDescending(orderBy);

        public IEnumerable<T> GetAll<TIncludeField, TSortField>(
            Expression<Func<T, ICollection<TIncludeField>>> include, Expression<Func<T, TSortField>> orderBy, bool ascending)
            => ascending ? Table.Include(include).OrderBy(orderBy) : Table.Include(include).OrderByDescending(orderBy);

        public IEnumerable<T> GetAll<TIncludeField, TSortField>(
            Expression<Func<T, TIncludeField>> include, Expression<Func<T, TSortField>> orderBy, bool ascending)
            => ascending ? Table.Include(include).OrderBy(orderBy) : Table.Include(include).OrderByDescending(orderBy);

        public IEnumerable<T> GetSome(Expression<Func<T, bool>> where) => Table.Where(where);

        public IEnumerable<T> GetSome<TIncludeField>(Expression<Func<T, bool>> where, Expression<Func<T, ICollection<TIncludeField>>> include)
            => Table.Where(where).Include(include);

        public IEnumerable<T> GetSome<TIncludeField>(Expression<Func<T, bool>> where, Expression<Func<T, TIncludeField>> include)
            => Table.Where(where).Include(include);

        public IEnumerable<T> GetSome<TSortField>(
            Expression<Func<T, bool>> where, Expression<Func<T, TSortField>> orderBy, bool ascending)
            => ascending ? Table.Where(where).OrderBy(orderBy) : Table.Where(where).OrderByDescending(orderBy);

        public IEnumerable<T> GetSome<TIncludeField, TSortField>(
            Expression<Func<T, bool>> where, Expression<Func<T, ICollection<TIncludeField>>> include,
            Expression<Func<T, TSortField>> orderBy, bool ascending)
            => ascending ?
            Table.Where(where).OrderBy(orderBy).Include(include) :
            Table.Where(where).OrderByDescending(orderBy).Include(include);

        public IEnumerable<T> GetSome<TIncludeField, TSortField>(
            Expression<Func<T, bool>> where, Expression<Func<T, TIncludeField>> include,
            Expression<Func<T, TSortField>> orderBy, bool ascending)
            => ascending ?
            Table.Where(where).OrderBy(orderBy).Include(include) :
            Table.Where(where).OrderByDescending(orderBy).Include(include);

        public virtual IEnumerable<T> GetRange(int skip, int take)
            => GetRange(Table, skip, take);

        public IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            entity.IsDeleted = true;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            foreach (T entity in entities)
            {
                entity.IsDeleted = true;
                Table.Update(entity);
            }
            return persist ? SaveChanges() : 0;
        }

        //internal T GetEntryFromChangeTracker(int? id)
        //{
        //    return Db.ChangeTracker.Entries<T>()
        //        .Select((EntityEntry e) => (T)e.Entity)
        //            .FirstOrDefault(x => x.Id == id);
        //}

        //public int Delete(int id, byte[] timeStamp, bool persist = true)
        //{
        //    var entry = GetEntryFromChangeTracker(id);
        //    if (entry != null)
        //    {
        //        if (entry.Timestamp.SequenceEqual(timeStamp))
        //        {
        //            return Delete(entry, persist);
        //        }
        //        throw new Exception("Unable to delete due to concurrency violation.");
        //    }
        //    Db.Entry(new T { Id = id, Timestamp = timeStamp }).State = EntityState.Deleted;
        //    return persist ? SaveChanges() : 0;
        //}

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

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }
            Db.Dispose();
            _disposed = true;
        }
    }
}