using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class ItemRepository : RepoBase<Item>, IItemRepository
    {
        public ItemRepository()
        {

        }

        public ItemRepository(DbContextOptions<OnlineStoreDBContext> options) : base(options)
        {

        }

        public IList<Item> GetByCategory(int categoryId)
        {
            return GetSome(x => x.Category.Id == categoryId && x.Deleted == false, x => x.Name, true).ToList();
        }
    }
}
