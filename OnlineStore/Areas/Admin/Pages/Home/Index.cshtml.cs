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
        public void OnGet()
        {
            NumberOfUsers = _userRepository.GetUsersByRole(CommonConstants.CustomerRoleId).Count;
            NumberOfEmployees = _userRepository.GetEmployees().Count;
            NumberOfProducts = _itemRepository.GetSome(i => i.IsDeleted == false).Count();
            NumberOfOrders = _orderRepository.GetSome(i => i.IsDeleted == false).Count();
        }
        public async Task<IActionResult> OnGetRevenue(string fromDate, string toDate)
        {
            var b = await _reportService.GetReportAsync(fromDate, toDate);
            return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
        }
    }
}