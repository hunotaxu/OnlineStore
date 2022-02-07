using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class WardRepository : BaseRepository<Ward>, IWardRepository
    {
        public WardRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}
