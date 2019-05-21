namespace OnlineStore.Models.ViewModels
{
    public class OrderDeliveryInfoViewModel
    {
        public OrderDeliveryInfoViewModel(string recipientFullName, string email, byte deliveryType, byte paymentType, byte address, byte shippingFee)
        {
            RecipientFullName = recipientFullName;
            Email = email;
            DeliveryType = deliveryType;
            PaymentType = paymentType;
            Address = address;
            ShippingFee = shippingFee;
        }

        public string RecipientFullName { get; set; }
        public string Email { get; set; }
        public byte DeliveryType { get; set; }
        public byte PaymentType { get; set; }
        public byte Address { get; set; }
        public byte ShippingFee { get; set; }
    }
}