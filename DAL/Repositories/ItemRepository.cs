using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;

namespace DAL.Repositories
{
    public class ItemRepository : RepoBase<Item>, IItemRepository
    {
        public ItemRepository()
        {

        }

        public ItemRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }

        public IList<Item> GetByCategory(int categoryId)
        {
            return GetSome(x => x.Category.Id == categoryId && x.Deleted == false, x => x.Name, true).ToList();
        }

        public IEnumerable<Item> GetItemByName(string name)
        {
            return Table.Where(s => s.Name.Contains(name));
        }
    }
}
