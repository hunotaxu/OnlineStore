using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Utilities.DTOs;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _lineItemRepository;
        private readonly MapperConfiguration _mapperConfiguration;

        public IndexModel(IOrderRepository orderRepository, IOrderItemRepository lineItemRepository)
        {
            _orderRepository = orderRepository;
            _lineItemRepository = lineItemRepository;
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Data.Entities.Order, OrderInfoViewModel>();
            });
        }

        [BindProperty]
        public DAL.Data.Entities.Order Order { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnGetAllPaging(byte? deliveryType, byte? orderStatus, string keyword, int pageIndex, int pageSize)
        {
            var order = _orderRepository.GetAllPaging(deliveryType, orderStatus, keyword, pageIndex, pageSize);
            var orderVM = _mapperConfiguration.CreateMapper().Map<PagedResult<OrderInfoViewModel>>(order);
            return new OkObjectResult(orderVM);
        }

        public IActionResult OnPostDelete([FromBody] DAL.Data.Entities.Order model)
        {
            var order = _orderRepository.Find(model.Id);
            _orderRepository.Delete(order);
            var lineItem = _lineItemRepository.GetSome(x => x.OrderId == model.Id && x.IsDeleted == false);
            _lineItemRepository.DeleteRange(lineItem);
            return new OkResult();
        }
    }
}