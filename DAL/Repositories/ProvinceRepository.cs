using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories  
{   
    public class ProvinceRepository : RepoBase<Province>, IProvinceRepository
    {
        public ProvinceRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}
