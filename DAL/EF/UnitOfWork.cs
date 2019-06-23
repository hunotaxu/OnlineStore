using DAL.Repositories;

namespace DAL.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineStoreDbContext _context;
        private readonly OrderRepository _orderRepository;
        public UnitOfWork(OnlineStoreDbContext context)
        {
            _context = context;
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
