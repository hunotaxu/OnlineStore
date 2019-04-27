using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;

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
    }
}
