using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAL.Repositories
{
    public class AddressRepository : RepoBase<Address>, IAddressRepository
    {
        public AddressRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
       

    }
}