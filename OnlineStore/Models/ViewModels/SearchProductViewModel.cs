using System.Collections.Generic;

namespace OnlineStore.Models.ViewModels
{
    public class SearchProductViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public byte Sort { get; set; }
        public string SearchString { get; set; }
        public List<string> Brand { get; set; }
        public int? Rating { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
