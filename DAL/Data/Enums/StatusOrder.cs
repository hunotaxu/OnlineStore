namespace DAL.Data.Enums
{
    public enum StatusOrder
    {
        Pending,
        ReadyToShip,        // Đang xử lý
        ReadyToReceive,      // Hàng đã sẵn sàng tại showroom, khách có thể đến nhận
        Shipped,            // Đang trên đường vận chuyển
        Delivered,          // Đã giao
        Canceled           // Đã hủy (do khách hàng hủy)
        //WaitingToReturn,   // Chờ để trả hàng (áp dụng cho chức năng đổi trả hàng hóa)
        //Returned            // Đã trả hàng
    }
}
