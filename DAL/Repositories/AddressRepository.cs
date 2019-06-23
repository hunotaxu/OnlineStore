using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AddressRepository : RepoBase<Address>, IAddressRepository
    {
        public AddressRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}