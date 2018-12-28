using System.Linq;
using DAL.EF;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AddressRepository : RepoBase<Address>, IAddressRepository
    {
        public AddressRepository()
        {

        }
        public AddressRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}
