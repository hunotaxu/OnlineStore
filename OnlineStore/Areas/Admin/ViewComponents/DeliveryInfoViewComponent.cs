using AutoMapper;
using Utilities.Commons;
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
        private readonly IAddressRepository _addressRepository;
        //private readonly IUserAddressRepository _userAddressRepository;

        //public DeliveryInfoViewComponent(IOrderRepository orderRepository, IUserAddressRepository userAddressRepository)
        public DeliveryInfoViewComponent(IOrderRepository orderRepository, IAddressRepository addressRepository)
        {
            _orderRepository = orderRepository;
            //_userAddressRepository = userAddressRepository;
            _addressRepository = addressRepository;
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<DAL.Data.Entities.Order, OrderDeliveryInfoViewModel>());
        }

        public Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            var order = _orderRepository.Find(orderId);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

            var userAddress = _addressRepository.Find(order.AddressId);
            var deliveryInfoVM = new OrderDeliveryInfoViewModel
            {
                RecipientFullName = order.Address.RecipientName,
                PhoneNumber = order.Address.PhoneNumber,
                Email = order.Address.Customer.Email,
                ReceivingTypeName = order.ReceivingType.Name,
                PaymentType = order.PaymentType,
                Status = order.Status,
                DeliveryDate = order.DeliveryDate ?? order.OrderDate,
                //ShippingFee = double.Parse(order.ShippingFee.ToString()).ToString("#,###", cul.NumberFormat)
                ShippingFee = CommonFunctions.FormatPrice(order.ShippingFee.Value.ToString())
            };
            //deliveryInfoVM.PhoneNumber = userAddress == null ? string.Empty : userAddress.PhoneNumber;
            deliveryInfoVM.Address = userAddress == null ? string.Empty : order.Address?.Detail;
            return Task.FromResult<IViewComponentResult>(View(deliveryInfoVM));
        }
    }
}