using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class MovieIndexViewModel
    {
        public IEnumerable<MovieViewModel> Movies { get; set; }
        public string SearchString { get; set; }
        public string MovieGenre { get; set; }
        public List<SelectListItem> MovieGenres { get; set; }
    }
}
