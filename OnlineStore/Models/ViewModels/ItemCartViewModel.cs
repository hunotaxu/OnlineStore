using OnlineStore.Models.ViewModels;

namespace OnlineStore.Models.ViewModels.Item
{
    public class ItemCartViewModel
    {
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Price { get; set; } // Dùng hàm CommonFunctions.FormatPrice(truyền vào decimal)
        public int Quantity { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
    }
}