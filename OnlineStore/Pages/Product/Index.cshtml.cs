using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        //public string CurrentFilter { get; set; }
        public SortType CurrentSort { get; set; }
        public string CurrentBrand { get; set; }
        public string CurrentSearchString { get; set; }
        public int? PageIndex { get; set; }

        public PaginatedList<Item> Items { get; set; }

        public IndexModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public void OnGet(string searchString)
        {
            List<Item> items = _itemRepository.GetSome(i => i.Name.Contains((searchString))).ToList();

            Items = PaginatedList<Item>.CreateAsync(items, 1, 5);

            //ViewData["Keyword"] = searchString;
            CurrentSearchString = searchString;

            //if (!Items.Any())
            //{
            //    return RedirectToPage("/Home/Index");
            //}

            //return View(lstSP.OrderBy(n => n.DonGia));
            //return Page();
        }

        public async Task<ActionResult> OnGetSearchAsync(SortType currentSort, List<string> currentBrand, decimal currentMinPrice,
            decimal currentMaxPrice, string currentRating, string currentSearchString, int? pageIndex)
        {
            List<Item> items = _itemRepository.GetSome(i => i.Name.Contains((currentSearchString))).ToList();
            //CurrentSort = sortOrder;
            //NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            //if (searchString != null)
            //{
            //    pageIndex = 1;
            //}
            //else
            //{
            //    searchString = currentSearch;
            //}

            //CurrentSearchString = searchString;

            //List<Item> itemResult = items;

            if (pageIndex != null)
            {
                PageIndex =  pageIndex;
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

            int pageSize = 5;
            Items = PaginatedList<Item>.CreateAsync(items, pageIndex ?? 1, pageSize);

            //var myViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { { "itemResult", itemResult } };
            var myViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "Items", Items } };
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