using AutoMapper;
using DAL.Data.Enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Admin.ViewComponents
{
    public class DeliveryInfoViewComponent : ViewComponent
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserAddressRepository _userAddressRepository;

        public DeliveryInfoViewComponent(IOrderRepository orderRepository, IUserAddressRepository userAddressRepository)
        {
            _orderRepository = orderRepository;
            _userAddressRepository = userAddressRepository;
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<DAL.Data.Entities.Order, OrderDeliveryInfoViewModel>());
        }

        public Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            var order = _orderRepository.Find(orderId);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var userAddress = _userAddressRepository.GetByUserAndAddress(order.CustomerId, order.AddressId.Value);
            var deliveryInfoVM = new OrderDeliveryInfoViewModel
            {
                RecipientFullName = order.Customer.Name,
                Email = order.Customer.Email,
                PhoneNumber = userAddress?.PhoneNumber,
                DeliveryType = (DeliveryType)order.DeliveryType,
                PaymentType = (PaymentType)order.PaymentType,
                DeliveryDate = order.DeliveryDate ?? order.OrderDate,
                //ShippingFee = order.ShippingFee,
                ShippingFee = double.Parse(order.ShippingFee.ToString()).ToString("#,###", cul.NumberFormat),
                AddressType = (byte)userAddress?.AddressType,
                Address = order.Address?.Detail
            };
            return Task.FromResult<IViewComponentResult>(View(deliveryInfoVM));
        }
    }
}