using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Utilities.DTOs;
using OnlineStore.Models.ViewModels;
using DAL.Data.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineStore.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IReceivingTypeRepository _receivingTypeRepository;
        private readonly MapperConfiguration _mapperConfiguration;

        public IndexModel(IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IReceivingTypeRepository receivingTypeRepository)
        {
            _orderRepository = orderRepository;
            _receivingTypeRepository = receivingTypeRepository;
            _orderItemRepository = orderItemRepository;
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Data.Entities.Order, OrderInfoViewModel>();
            });
        }

        [BindProperty]
        public DAL.Data.Entities.Order Order { get; set; }
        [BindProperty]
        public IEnumerable<ReceivingType> ReceivingTypes { get; set; }

        public void OnGet()
        {
            ReceivingTypes = _receivingTypeRepository.GetAll();
        }

        public IActionResult OnGetAllPaging(byte? receivingTypeId, byte? orderStatus, string keyword, int pageIndex, int pageSize)
        {
            ReceivingTypes = _receivingTypeRepository.GetAll();
            var order = _orderRepository.GetAllPaging(receivingTypeId, orderStatus, keyword, pageIndex, pageSize);
            var orderVM = _mapperConfiguration.CreateMapper().Map<PagedResult<OrderInfoViewModel>>(order);
            return new OkObjectResult(orderVM);
        }

        public IActionResult OnPostDelete([FromBody] DAL.Data.Entities.Order model)
        {
            var order = _orderRepository.Find(model.Id);
            _orderRepository.Delete(order);
            var lineItem = _orderItemRepository.GetSome(x => x.OrderId == model.Id && x.IsDeleted == false);
            _orderItemRepository.DeleteRange(lineItem);
            return new OkResult();
        }
    }
}