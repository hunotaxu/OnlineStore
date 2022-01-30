using DAL.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.ViewModels
{
    public class OrderInfoViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public virtual ReceivingType ReceivingType { get; set; }
        public byte Status { get; set; }
    }
}