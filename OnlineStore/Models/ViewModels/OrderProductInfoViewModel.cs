namespace OnlineStore.Models.ViewModels
{
    public class OrderProductInfoViewModel
    {
        public string Image { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public decimal SaleOff { get; set; }
        public string Amount { get; set; }
    }
}