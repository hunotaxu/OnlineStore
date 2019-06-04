using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using TimiApp.Dapper.Interfaces;
using Utilities.Commons;

namespace OnlineStore.Areas.Admin.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;
        public IndexModel(IReportService reportService, IOrderRepository orderRepository, IUserRepository userRepository, IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _reportService = reportService;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
        }
        public long NumberOfUsers { get; set; }
        public long NumberOfEmployees { get; set; }
        public long NumberOfProducts { get; set; }
        public long NumberOfOrders { get; set; }
        public void OnGet()
        {
            NumberOfUsers = _userRepository.GetUsersByRole(CommonConstants.CustomerRoleId).Count;
            NumberOfEmployees = _userRepository.GetEmployees().Count;
            NumberOfProducts = _itemRepository.GetSome(i => i.IsDeleted == false).Count();
            NumberOfOrders = _orderRepository.GetSome(i => i.IsDeleted == false).Count();
            var listOrderDelivered = _orderRepository.GetSome(x => x.Status == DAL.Data.Enums.OrderStatus.Delivered).SelectMany(o => o.LineItems);
            var items = _itemRepository.GetSome(c => (listOrderDelivered.Any(x => x.ItemId == c.Id)));
            //var category = _categoryRepository.GetSome(c=>c.Item.Id)
        }
        public async Task<IActionResult> OnGetRevenue(string fromDate, string toDate)
        {
            return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
        }
    }
}