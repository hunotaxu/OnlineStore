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

            var userAddress = order.AddressId.HasValue == true ? _userAddressRepository.GetByUserAndAddress(order.CustomerId, order.AddressId.Value) : null;

            var deliveryInfoVM = new OrderDeliveryInfoViewModel
            {
                RecipientFullName = order.Customer.Name,
                Email = order.Customer.Email,
                DeliveryType = order.DeliveryType,
                PaymentType = order.PaymentType,
                Status = order.Status,
                DeliveryDate = order.DeliveryDate ?? order.OrderDate,
                ShippingFee = double.Parse(order.ShippingFee.ToString()).ToString("#,###", cul.NumberFormat)
            };
            deliveryInfoVM.PhoneNumber = userAddress == null ? string.Empty : userAddress.PhoneNumber;
            //deliveryInfoVM.AddressType = userAddress == null ? (byte)0 : userAddress.AddressType;
            deliveryInfoVM.Address = userAddress == null ? string.Empty : order.Address?.Detail;
            return Task.FromResult<IViewComponentResult>(View(deliveryInfoVM));
        }
    }
}