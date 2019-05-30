using OnlineStore.Models.ViewModels;

namespace OnlineStore.Models.ViewModels.Item
{
    public class ItemCartViewModel
    {
        public ItemViewModel Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public ItemCartViewModel()
        {


        }

    }
}