using DAL.Data.Enums;
using System;

namespace OnlineStore.Models.ViewModels
{
    public class OrderEmailViewModel
    {
        public string Text { get; set; } = "Xem chi tiết đơn hàng";
        public int OrderId { get; set; }
        public string LetterDescription { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; } 
        public string Url { get; set; }
    }
}
