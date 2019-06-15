using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class DistrictRepository : RepoBase<District>, IDistrictRepository
    {
        public DistrictRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}
