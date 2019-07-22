using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Enums
{
    public enum OrderStatus : byte
    {
        [Display(Name="Tất cả trạng thái đơn hàng")]
        None,
        [Display(Name = "Đang chờ xử lý")]
        Pending,
        [Display(Name = "Sẵn sàng để giao")]
        ReadyToDeliver,
        //[Display(Name = "Sẵn sàng nhận tại showroom")]
        //ReadyToReceive,
        [Display(Name = "Đang vận chuyển")]
        Shipped,
        [Display(Name = "Đã giao")]
        Delivered,
        [Display(Name = "Đã hủy")]
        Canceled
        //WaitingToReturn,   // Chờ để trả hàng (áp dụng cho chức năng đổi trả hàng hóa)
        //Returned            // Đã trả hàng
    }
}