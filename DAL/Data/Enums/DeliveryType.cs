using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Enums
{
    public enum DeliveryType
    {
        [Display(Name = "Chọn phương thức giao hàng")]
        None,
        [Display(Name = "Giao hàng chuẩn")]
        StandardShipping, // Giao trong vòng 2 - 4 ngày
        [Display(Name = "Giao hàng ưu tiên")]
        PriorityShipping, // Giao trong ngày hôm sau
        [Display(Name = "Nhận hàng tại showroom")]
        SelfPickup        // Tự nhận hàng tại showroom
    }
}