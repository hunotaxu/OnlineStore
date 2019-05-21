//using AutoMapper;
//using DAL.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using OnlineStore.Models.ViewModels;
//using System.Threading.Tasks;

//namespace OnlineStore.Areas.Admin.ViewComponents
//{
//    public class DeliveryInfoViewComponent : ViewComponent
//    {
//        private readonly MapperConfiguration _mapperConfiguration;
//        private readonly IOrderRepository _orderRepository;

//        public DeliveryInfoViewComponent(IOrderRepository orderRepository, )
//        {
//            _orderRepository = orderRepository;
//            _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<DAL.Data.Entities.Order, OrderDeliveryInfoViewModel>());
//        }

//        public Task<IViewComponentResult> InvokeAsync(int orderId)
//        {
//            //var deliveryInfoVM = _mapperConfiguration.CreateMapper().Map<OrderDeliveryInfoViewModel>(_orderRepository.Find(orderId));
//            var order = _orderRepository.Find(orderId);

//            //var deliveryInfoVM = new OrderDeliveryInfoViewModel
//            //{
//            //    Address
//            //};
//            return Task.FromResult<IViewComponentResult>(View(deliveryInfoVM));
//        }
//    }
//}