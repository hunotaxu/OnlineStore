using DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Event : EntityBase
    {
        public Event()
        {
            Item = new HashSet<Item>();
        }

        //public int Id { get; set; }
        [Required]
        [Display(Name="Tên sự kiện")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name="Hình ảnh sự kiện")]
        public byte[] Image { get; set; }

        [Display(Name="Ưu đãi")]
        public decimal? Bonus { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}
