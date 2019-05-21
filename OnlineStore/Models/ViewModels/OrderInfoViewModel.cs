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
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy HH:MM}")]
        public DateTime DeliveryDate { get; set; }
        public byte Status { get; set; }
    }
}