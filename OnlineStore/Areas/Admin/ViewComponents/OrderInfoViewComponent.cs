using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace OnlineStore.Areas.ViewComponents
{
    public class OrderInfoViewComponent : ViewComponent
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderInfoViewComponent(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            DAL.Data.Entities.Order model = _orderRepository.Find(orderId);
            OrderInfoViewModel vm = _mapper.Map<OrderInfoViewModel>(model);
            return Task.FromResult<IViewComponentResult>(View(vm));
        }
    }
}
