using DAL.Repositories;
using System;

namespace DAL.EF
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IAddressRepository AddressRepository { get; }
        ICartRepository CartRepository { get; }
        IItemRepository ItemRepository { get; }
        ICartDetailRepository CartDetailRepository { get; }
        IReceivingTypeRepository ReceivingTypeRepository { get; }

        void Save();

        void Commit();

        void Rollback();
    }
}