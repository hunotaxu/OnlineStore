using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Event : EntityBase
    {
        public Event()
        {
            Item = new HashSet<Item>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tên sự kiện")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Ưu đãi")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaleOff { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        //public byte[] Timestamp { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}