using DAL.Models;
using OnlineStore.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class MovieViewModel : Movie
    {
        public new string Genre { get; set; }

        [DateMustBeGreaterThan("12/31/1969")]
        public override DateTime ReleaseDate { get; set; }
    }
}