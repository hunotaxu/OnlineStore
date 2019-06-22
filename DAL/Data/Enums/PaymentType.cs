using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Enums
{
    public enum PaymentType : byte
    {
        [Display(Name = "Thanh toán khi nhận hàng")]
        CashOnDelivery,
        [Display(Name= "Thanh toán bằng thẻ quốc tế")]
        CreditDebitCard,
        //[Display(Name = "Thanh toán bằng thẻ ATM nội địa")]
        //LocalATM,
        [Display(Name = "Thanh toán qua momo")]
        Momo
    }
}