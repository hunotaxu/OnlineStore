using DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.ViewModels.Item
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int Quantity { get; set; }

        public decimal? AverageEvaluation { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        [Required]
        [DefaultValue(0)]
        public decimal? PromotionPrice { get; set; }
        public decimal? OriginalPrice { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? View { get; set; }

        public string BrandName { get; set; }

        public CategoryViewModel Category { set; get; }
        public IList<ProductImages> ProductImages { get; set; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public byte? Status { set; get; }
    }
}
