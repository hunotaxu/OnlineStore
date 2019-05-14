using System;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using Utilities.DTOs;
using OfficeOpenXml;
using System.IO;

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

        public void ImportExcel(string filePath, int categoryId)
        {
            // sau khi using kết thúc thì sẽ tự động dispose cái vùng nhớ
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Item item;
                for (int i = workSheet.Dimension.Start.Row + 1; i < workSheet.Dimension.End.Row; i++)
                {
                    item = new Item();
                    item.CategoryId = categoryId;
                    item.Name = workSheet.Cells[i, 1].Value.ToString();
                    item.Description = workSheet.Cells[i, 2].Value.ToString();
                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var price); // Trong C# 7 không cần khai báo trước price
                    item.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var promotionPrice);
                    item.PromotionPrice = promotionPrice;
                    Table.Add(item);
                }
            }
        }
    }
}