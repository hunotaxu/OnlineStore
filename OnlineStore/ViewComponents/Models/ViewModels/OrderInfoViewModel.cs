using DAL.Data.Entities;
using System;

namespace OnlineStore.Models.ViewModels
{
    public class OrderInfoViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        //public byte ReceivingType { get; set; }
        public virtual ReceivingType ReceivingType { get; set; }
        public byte Status { get; set; }
    }
}