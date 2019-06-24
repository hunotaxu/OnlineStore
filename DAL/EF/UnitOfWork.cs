using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private OnlineStoreDbContext _context;
        private IOrderRepository _orderRepository;
        DbContextOptions<OnlineStoreDbContext> _option;
        public UnitOfWork(OnlineStoreDbContext context,
            DbContextOptions<OnlineStoreDbContext> option)
        {
            _option = option;
            _context = context;
        }
        public IOrderRepository OrderRepository
        {
            get
            {
                if(_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_option);
                }
                return _orderRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
