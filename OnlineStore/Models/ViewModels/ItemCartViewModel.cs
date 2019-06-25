namespace OnlineStore.Models.ViewModels.Item
{
    public class ItemCartViewModel
    {
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}