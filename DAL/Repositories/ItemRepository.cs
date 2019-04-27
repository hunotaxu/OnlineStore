using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public class ItemRepository : RepoBase<Item>, IItemRepository
    {
        public ItemRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }

        public IEnumerable<Item> GetByCategory(int categoryId)
        {
            return GetSome(x => x.Category.Id == categoryId && x.IsDeleted == false, x => x.Name, true).ToList();
        }

        public Item GetItem(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Item> GetItemByName(string name)
        {
            return Table.Where(s => s.Name.Contains(name));
        }

        public PagedResult<Item> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = GetSome(i => i.IsDeleted == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(i => i.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query.Where(x => x.CategoryId == categoryId.Value);
            }

            var totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);

            var paginationSet = new PagedResult<Item>
            {
                Results = query.ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }
    }
}
