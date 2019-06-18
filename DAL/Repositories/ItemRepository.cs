using System;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using Utilities.DTOs;
using OfficeOpenXml;
using System.IO;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class ItemRepository : RepoBase<Item>, IItemRepository
    {
        private readonly ILogger<ItemRepository> _logger;
        //public ItemRepository(ILogger logger)
        //{
        //    _logger = logger;
        //}
        public ItemRepository(DbContextOptions<OnlineStoreDbContext> options, ILogger<ItemRepository> logger) : base(options)
        {
            _logger = logger;
        }

        public IEnumerable<Item> GetByCategory(int categoryId)
        {
            return GetSome(x => x.Category.Id == categoryId && x.IsDeleted == false, x => x.Name, true).ToList();
        }

        public IEnumerable<Item> GetItemByName(string name)
        {
            return Table.Where(s => s.Name.Contains(name));
        }

        public PagedResult<Item> GetAllPaging(int? categoryId, string keyword, int pageIndex, int pageSize)
        {
            var query = GetSome(i => i.IsDeleted == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(i => i.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            var rowCount = query.ToList().Count;

            query = query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var paginationSet = new PagedResult<Item>
            {
                Results = query.ToList(),
                CurrentPage = pageIndex,
                RowCount = rowCount,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool ImportExcel(string filePath, int categoryId)
        {
            try
            {
                // sau khi using kết thúc thì sẽ tự động dispose cái vùng nhớ
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                    Item item;
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        item = new Item();
                        item.CategoryId = categoryId;
                        item.Name = workSheet.Cells[i, 1].Value.ToString();
                        item.Description = workSheet.Cells[i, 2].Value.ToString();
                        item.BrandName = workSheet.Cells[i, 3].Value.ToString();
                        decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var quantity);
                        item.Quantity = Convert.ToInt32(quantity);
                        decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var price); // Trong C# 7 không cần khai báo trước price
                        item.Price = price;
                        decimal.TryParse(workSheet.Cells[i, 6].Value.ToString(), out var promotionPrice);
                        item.PromotionPrice = promotionPrice;
                        item.DateCreated = DateTime.Now;
                        Add(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot import file", ex.Message);
                return false;
            }
        }
    }
}