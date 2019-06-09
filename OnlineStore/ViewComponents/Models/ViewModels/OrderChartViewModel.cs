using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace OnlineStore.Models.ViewModels
{
    public class OrderChartViewModel
    {
        public OrderChartViewModel(string month, decimal revence)
        {
            Month = month;
            Revence = revence;
        }

        public string Month { get; set; }
        public decimal Revence { get; set; }

    }
}
