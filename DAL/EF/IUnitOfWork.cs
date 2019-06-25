using DAL.Repositories;
using System;

namespace DAL.EF
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository OrderRepository { get; }
        /// <summary>
        /// Call save change from db context
        /// </summary>
        void Save();
    }
}
