using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    class ReceivingTypeResponsitory : RepoBase<ReceivingType>, IReceivingTypeRepository
    {
        public ReceivingTypeResponsitory(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}