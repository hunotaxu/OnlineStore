namespace TimiApp.Dapper.ViewModels
{
    public class BestSellerOfCategoryViewModel
    {
        public string CategoryName { get; set; }
        public string[] Colors { get; set; } = new string[] { "blue", "green", "purple" };
        public int NumberOfDeliverdItems { get; set; }
    }
}
