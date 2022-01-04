using System;
using DAL.Repositories.Base;
using System.Collections.Generic;
using DAL.Data.Entities;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public interface IItemRepository : IRepo<Item>
    {
        IEnumerable<Item> GetByCategory(int categoryId);
        IEnumerable<Item> GetItemByName(string name);
        void UpdateWithoutSave(Item item);
        PagedResult<Item> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
        PagedResult<Item> GetAllPaging(decimal? maxPrice, decimal? minPrice, int? categoryId, int? rating, byte sortType, string searchString, List<string> brandNames, int pageIndex, int pageSize);
        bool ImportExcel(string filePath, int categoryId);

    }
}