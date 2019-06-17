namespace TimiApp.Dapper.ViewModels
{
    public class MostReceivingMethodViewModel
    {
        public string ReceivingTypeName { get; set; }
        public string[] Colors { get; set; } = new string[] { "#BDC3C7", "#9B59B6", "#E74C3C", "#EE204D" };
        public double ProportionOfDeliverdItems { get; set; }
    }
}