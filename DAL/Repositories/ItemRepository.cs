using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(DbContextOptions<OnlineStoreDbContext> options, OnlineStoreDbContext context = null) : base(options, context)
        {
        }

        public IEnumerable<Item> GetByCategory(int categoryId)
        {
            return GetSome(x => x.Category.Id == categoryId && x.IsDeleted == false, x => x.Name, true).ToList();
        }

        public IEnumerable<Item> GetItemByName(string name)
        {
            return Table.Where(s => s.Name.Contains(name));
        }

        public PagedResult<Item> GetAllPaging(decimal? maxPrice, decimal? minPrice, int? categoryId, int? rating, byte sortType, string searchString, List<string> brandNames, int pageIndex, int pageSize)
        {
            var query = GetSome(i => i.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(i => i.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) && i.Quantity > 0);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == categoryId);
            }

            if (brandNames?.Any() == true)
            {
                List<Item> itemFilters = query.Where(x => brandNames.Contains(x.BrandName)).ToList();
                query = itemFilters;
            }

            switch ((SortType)sortType)
            {
                case SortType.PriceLowToHigh:
                    query = new List<Item>(query.OrderBy(s => s.Price));
                    break;

                case SortType.PriceHighToLow:
                    query = new List<Item>(query.OrderByDescending(s => s.Price));
                    break;

                case SortType.Popularity:
                    break;

                default:
                    query = new List<Item>(query.OrderByDescending(s => s.View));
                    break;
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(x => decimal.Compare(x.Price, maxPrice.Value) <= 0);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(x => decimal.Compare(x.Price, minPrice.Value) >= 0);
            }

            if (rating.HasValue)
            {
                query = query.Where(x => x.AverageEvaluation >= rating.Value);
            }

            var rowCount = query.ToList().Count;

            var All = query.ToList();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var paginationSet = new PagedResult<Item>
            {
                All = All,
                Results = query.ToList(),
                CurrentPage = pageIndex,
                RowCount = rowCount,
                PageSize = pageSize
            };

            return paginationSet;
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

        public void UpdateWithoutSave(Item item)
        {
            Table.Update(item);
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
                        item = new Item
                        {
                            CategoryId = categoryId,
                            SKU = workSheet.Cells[i, 1].Value.ToString(),
                            Name = workSheet.Cells[i, 2].Value.ToString(),
                            Description = workSheet.Cells[i, 3].Value.ToString(),
                            BrandName = workSheet.Cells[i, 4].Value.ToString()
                        };
                        decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var quantity);
                        item.Quantity = Convert.ToInt32(quantity);
                        decimal.TryParse(workSheet.Cells[i, 6].Value.ToString(), out var price); // Trong C# 7 không cần khai báo trước price
                        item.Price = price;
                        decimal.TryParse(workSheet.Cells[i, 7].Value.ToString(), out var promotionPrice);
                        item.OriginalPrice = promotionPrice;
                        item.DateCreated = DateTime.Now;
                        Add(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}