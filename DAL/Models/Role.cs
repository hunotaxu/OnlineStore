using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Role : EntityBase
    {
        public Role()
        {
            UserDecentralizations = new HashSet<UserDecentralization>();
        }

        //public int Id { get; set; }
        [Column(TypeName="nvarchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        public ICollection<UserDecentralization> UserDecentralizations { get; set; }
    }
}
