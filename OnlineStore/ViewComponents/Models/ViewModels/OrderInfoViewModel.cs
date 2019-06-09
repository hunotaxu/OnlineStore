using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.ViewModels
{
    public class OrderInfoViewModel
    {
        public int Id { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:MM}")]
        //[DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public byte DeliveryType { get; set; }
        public byte Status { get; set; }
    }
}