using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace OnlineStore.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        public SortType CurrentSort { get; set; }
        public string CurrentSearchString { get; set; }
        public int CurrentPage { get; set; }
        public PaginatedList<Item> Items { get; set; }

        public IndexModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public void OnGet(string searchString)
        {
            List<Item> items = _itemRepository.GetSome(i => i.Name.Contains((searchString)))
                .OrderByDescending(s => s.View).ToList();
            CurrentPage = 1;
            Items = PaginatedList<Item>.CreateAsync(items, CurrentPage, 6);
            CurrentSearchString = searchString;
        }

        public void OnGetCategory(int categoryId)
        {
            List<Item> items = _itemRepository.GetSome(i => i.CategoryId == categoryId).OrderByDescending(s => s.View).ToList();
            CurrentPage = 1;
            Items = PaginatedList<Item>.CreateAsync(items, CurrentPage, 6);
        }

        public async Task<ActionResult> OnGetSearchAsync(SortType currentSort, List<string> currentBrand, decimal currentMinPrice,
            decimal currentMaxPrice, string currentRating, string currentSearchString, int? currentPage)
        {
            List<Item> items = _itemRepository.GetSome(i => i.Name.Contains((currentSearchString))).ToList();
            if (currentPage != null)
            {
                CurrentPage = currentPage.Value;
            }
            if (currentBrand.Any())
            {
                if (currentBrand.Count != items.Count)
                {
                    List<Item> itemFilters = (from c in currentBrand
                                              from i in items
                                              where (i.BrandName.Equals(c))
                                              select i).ToList();
                    items = itemFilters;
                }
            }

            switch (currentSort)
            {
                case SortType.PriceLowToHigh:
                    items = new List<Item>(items.OrderBy(s => s.Price));
                    break;
                case SortType.PriceHighToLow:
                    items = new List<Item>(items.OrderByDescending(s => s.Price));
                    break;
                default:
                    items = new List<Item>(items.OrderByDescending(s => s.View));
                    break;
            }

            int pageSize = 6;
            Items = PaginatedList<Item>.CreateAsync(items, currentPage ?? 1, pageSize);

            var myViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { { "Items", Items } };
            myViewData.Model = Items;

            PartialViewResult result = new PartialViewResult()
            {
                ViewName = "_SearchResultGridPartial",
                ViewData = myViewData
            };

            return result;
        }
    }
}