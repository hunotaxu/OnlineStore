using DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class TypeOfUser : EntityBase
    {
        public TypeOfUser()
        {
            Users = new HashSet<User>();
            UserDecentralizations = new HashSet<UserDecentralization>();
        }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<UserDecentralization> UserDecentralizations { get; set; }
    }
}
