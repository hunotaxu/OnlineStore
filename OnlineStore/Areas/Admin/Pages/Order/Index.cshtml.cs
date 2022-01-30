using AutoMapper;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using Utilities.DTOs;

namespace OnlineStore.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IReceivingTypeRepository _receivingTypeRepository;
        private readonly IMapper _mapper;

        public IndexModel(IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IReceivingTypeRepository receivingTypeRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _receivingTypeRepository = receivingTypeRepository;
            _orderItemRepository = orderItemRepository;
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
            PagedResult<DAL.Data.Entities.Order> order = _orderRepository.GetAllPaging(receivingTypeId, orderStatus, keyword, pageIndex, pageSize);
            PagedResult<OrderInfoViewModel> orderVM = _mapper.Map<PagedResult<OrderInfoViewModel>>(order);
            return new OkObjectResult(orderVM);
        }

        public IActionResult OnPostDelete([FromBody] DAL.Data.Entities.Order model)
        {
            DAL.Data.Entities.Order order = _orderRepository.Find(model.Id);
            _orderRepository.Delete(order);
            List<OrderItem> lineItem = _orderItemRepository.GetSome(x => x.OrderId == model.Id && x.IsDeleted == false);
            _orderItemRepository.DeleteRange(lineItem);
            return new OkResult();
        }
    }
}