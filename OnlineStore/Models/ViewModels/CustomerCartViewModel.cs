namespace OnlineStore.Models.ViewModels
{
    public class CustomerCartViewModel
    {
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string ItemName { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
