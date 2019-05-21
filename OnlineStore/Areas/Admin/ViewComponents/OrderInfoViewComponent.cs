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
        private readonly MapperConfiguration _mapper;

        public OrderInfoViewComponent(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<DAL.Data.Entities.Order, OrderInfoViewModel>());
        }

        public Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            var model = _orderRepository.Find(orderId);
            var vm = _mapper.CreateMapper().Map<OrderInfoViewModel>(model);
            return Task.FromResult<IViewComponentResult>(View(vm));
        }
    }
}
