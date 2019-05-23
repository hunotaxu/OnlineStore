using DAL.Data.Enums;
using System;

namespace OnlineStore.Models.ViewModels
{
    public class OrderDeliveryInfoViewModel
    {
        public string RecipientFullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public DateTime DeliveryDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Address { get; set; }
        public byte AddressType { get; set; }
        //public decimal? ShippingFee { get; set; }
        public string ShippingFee { get; set; }
    }
}