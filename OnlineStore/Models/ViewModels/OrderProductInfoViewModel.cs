using DAL.Data.Enums;
using System;

namespace OnlineStore.Models.ViewModels
{
    public class OrderProductInfoViewModel
    {
        //public string RecipientFullName { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public decimal SaleOff { get; set; }
        public string Amount { get; set; }
        //public DeliveryType DeliveryType { get; set; }
        //public DateTime DeliveryDate { get; set; }
        //public PaymentType PaymentType { get; set; }
        //public string Address { get; set; }
        //public byte AddressType { get; set; }
        ////public decimal? ShippingFee { get; set; }
        //public string ShippingFee { get; set; }
    }
}