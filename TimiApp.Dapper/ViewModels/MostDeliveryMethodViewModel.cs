using DAL.Data.Entities;
using DAL.Data.Enums;
using Utilities.Extensions;

namespace TimiApp.Dapper.ViewModels
{
    public class MostDeliveryMethodViewModel
    {
        private DeliveryType delivery { get; set; }
        //public string DeliveryName
        //{
        //    get
        //    {
        //        return deliveryName.GetDisplayName();
        //    }
        //    set
        //    {
                
        //    }
        //}
        public string[] Colors { get; set; } = new string[] { "#BDC3C7", "#9B59B6", "#E74C3C", "#EE204D" };
        public double ProportionOfDeliverdItems { get; set; }
    }
}