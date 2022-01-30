using AutoMapper;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models.ViewModels.Item;
using Utilities.DTOs;

namespace OnlineStore.Pages.Product
{
    public class SearchModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public SearchProductViewModel SearchProductViewModel { get; set; }
        public SearchModel(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public void OnGet()
        {

        }

        public void OnGetCategory(int categoryId)
        {
            SearchProductViewModel.CategoryId = categoryId;
        }

        public IActionResult OnGetAllPaging()
        {
            PagedResult<Item> model = _itemRepository.GetAllPaging(SearchProductViewModel.MaxPrice, SearchProductViewModel.MinPrice,
                SearchProductViewModel.CategoryId, SearchProductViewModel.Rating, SearchProductViewModel.Sort, SearchProductViewModel.SearchString,
                SearchProductViewModel.Brand, SearchProductViewModel.PageIndex, SearchProductViewModel.PageSize);
            PagedResult<ItemViewModel> itemsPagination = _mapper.Map<PagedResult<ItemViewModel>>(model);
            return new OkObjectResult(itemsPagination);
        }
    }
}