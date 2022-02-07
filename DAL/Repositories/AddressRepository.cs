using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(DbContextOptions<OnlineStoreDbContext> options, OnlineStoreDbContext context = null) : base(options, context)
        {
        }
    }
}