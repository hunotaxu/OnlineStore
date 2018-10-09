using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class MovieGenre : EntityBase
    {
        public string GenreName { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
