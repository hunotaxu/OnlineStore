using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Enums
{
    public enum SortType
    {
        [Display(Name="Phổ biến nhất")]
        Popularity,
        [Display(Name = "Giá từ thấp lên cao")]
        PriceLowToHigh,
        [Display(Name = "Giá từ cao xuống thấp")]
        PriceHighToLow
    }
}
