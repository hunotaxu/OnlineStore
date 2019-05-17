using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Enums
{
    public enum OrderStatus
    {
        [Display(Name="Chọn trạng thái đơn hàng")]
        None,
        [Display(Name = "Đang chờ xử lý")]
        Pending,            // Đang chờ xử lý
        [Display(Name = "Sẵn sàng để giao")]
        ReadyToShip,        // Sẵn sàng được giao
        [Display(Name = "Sẵn sàng để nhận tại showroom")]
        ReadyToReceive,      // Hàng đã sẵn sàng tại showroom, khách có thể đến nhận
        [Display(Name = "Đang vận chuyển")]
        Shipped,            // Đang trên đường vận chuyển
        [Display(Name = "Đã giao")]
        Delivered,          // Đã giao <=> khách đã nhận (với trường hợp ở showroom)
        [Display(Name = "Đã hủy")]
        Canceled           // Đã hủy (do khách hàng hủy)
        //WaitingToReturn,   // Chờ để trả hàng (áp dụng cho chức năng đổi trả hàng hóa)
        //Returned            // Đã trả hàng
    }
}