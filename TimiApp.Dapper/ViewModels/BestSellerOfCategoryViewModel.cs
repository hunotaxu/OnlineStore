namespace TimiApp.Dapper.ViewModels
{
    public class BestSellerOfCategoryViewModel
    {
        public string CategoryName { get; set; }
        public string[] Colors { get; set; } = new string[] { "#BDC3C7", "#9B59B6", "#E74C3C" };
        public int NumberOfDeliverdItems { get; set; }
    }
}
