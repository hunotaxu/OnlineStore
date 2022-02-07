using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.EF
{
    /// <summary>
    /// https://gist.github.com/azborgonovo/445ef9a864ea9ec38a2e
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;
        private IItemRepository _itemRepository;
        private ICartRepository _cartRepository;
        private IAddressRepository _addressRepository;
        private IReceivingTypeRepository _receivingTypeRepository;
        private ICartDetailRepository _cartDetailRepository;
        private readonly OnlineStoreDbContext _context;
        public IDbContextTransaction _transaction;
        private readonly DbContextOptions<OnlineStoreDbContext> _option;

        public UnitOfWork(OnlineStoreDbContext context,
            DbContextOptions<OnlineStoreDbContext> option)
        {
            _option = option;
            _context = context;
            _transaction = context.Database.BeginTransaction();
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_option, _context);
                }
                return _orderRepository;
            }
        }

        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                if (_orderItemRepository == null)
                {
                    _orderItemRepository = new OrderItemRepository(_option, _context);
                }
                return _orderItemRepository;
            }
        }

        public IItemRepository ItemRepository
        {
            get
            {
                if (_itemRepository == null)
                {
                    _itemRepository = new ItemRepository(_option, _context);
                }
                return _itemRepository;
            }
        }

        public ICartRepository CartRepository
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_option, _context);
                }
                return _cartRepository;
            }
        }

        public IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                {
                    _addressRepository = new AddressRepository(_option, _context);
                }
                return _addressRepository;
            }
        }

        public IReceivingTypeRepository ReceivingTypeRepository
        {
            get
            {
                if (_receivingTypeRepository == null)
                {
                    _receivingTypeRepository = new ReceivingTypeRepository(_option, _context);
                }
                return _receivingTypeRepository;
            }
        }

        public ICartDetailRepository CartDetailRepository
        {
            get
            {
                if (_cartDetailRepository == null)
                {
                    _cartDetailRepository = new CartDetailRepository(_option, _context);
                }
                return _cartDetailRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Commit()
        {
            _context.SaveChanges();
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (_context != null && _transaction != null)
            {
                _context.Dispose();
                _transaction.Dispose();
            }
        }
    }
}