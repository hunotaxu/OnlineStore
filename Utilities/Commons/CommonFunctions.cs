using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Globalization;

namespace Utilities.Commons
{
    public class CommonFunctions
    {
        public static string FormatNumber(decimal number, int precision)
        {
            var a = number.ToString($"N{precision}").Split('.');
            a[0] = a[0].Replace("/\\d(?=(\\d{3})+$)/g", "$&,");
            return a.Join(".");
        }
        public static string FormatPrice(decimal price)
        {
            return FormatNumber(price, 0) + " VNĐ";
        }
        public static string FormatPrice(string strPrice)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            return double.Parse(strPrice).ToString("#,###", cul.NumberFormat);
        }
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
