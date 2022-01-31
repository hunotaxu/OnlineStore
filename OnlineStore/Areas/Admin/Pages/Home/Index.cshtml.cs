using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimiApp.Dapper.Interfaces;
using TimiApp.Dapper.ViewModels;
using Utilities.Commons;

namespace OnlineStore.Areas.Admin.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IOrderRepository _orderRepository;
        public IndexModel(IReportService reportService, IOrderRepository orderRepository, IUserRepository userRepository, IItemRepository itemRepository)
        {
            _reportService = reportService;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
        }
        public long NumberOfUsers { get; set; }
        public long NumberOfEmployees { get; set; }
        public long NumberOfProducts { get; set; }
        public long NumberOfOrders { get; set; }
        public IEnumerable<BestSellerOfCategoryViewModel> BestSoldCategories { get; set; }
        public long NumberOfDeliveredItem { get; set; }
        public long NumberOfOtherDeliveredItem { get; set; }
        public async Task OnGetAsync()
        {
            NumberOfUsers = _userRepository.GetUsersByRole(CommonConstants.CustomerRoleId).Count;
            NumberOfEmployees = _userRepository.GetEmployees().Count;
            NumberOfProducts = _itemRepository.GetSome(i => i.IsDeleted == false).Count();
            NumberOfOrders = _orderRepository.GetSome(i => i.IsDeleted == false).Count();
            var listOrderDelivered = _orderRepository.GetSome(x => x.Status == DAL.Data.Enums.OrderStatus.Delivered).SelectMany(o => o.OrderItems);
            var items = _itemRepository.GetSome(c => listOrderDelivered.Any(x => x.ItemId == c.Id));
            BestSoldCategories = await _reportService.GetBestSellerOfCategory();
            NumberOfDeliveredItem = BestSoldCategories.Sum(x => x.NumberOfDeliverdItems);
            NumberOfOtherDeliveredItem = NumberOfDeliveredItem - BestSoldCategories.Take(3).Sum(x => x.NumberOfDeliverdItems);
        }
        public async Task<IActionResult> OnGetCategories()
        {
            BestSoldCategories = await _reportService.GetBestSellerOfCategory();
            return new OkObjectResult(await _reportService.GetBestSellerOfCategory());
        }
        public async Task<IActionResult> OnGetDeliverMethod()
        {
            IEnumerable<MostReceivingMethodViewModel> result = await _reportService.GetTopMostOfCategoryAsync();
            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnGetRevenue(string fromDate, string toDate)
        {
            IEnumerable<RevenueReportViewModel> result = await _reportService.GetRevenueReportAsync(fromDate, toDate);
            return new OkObjectResult(result);
        }
    }
}