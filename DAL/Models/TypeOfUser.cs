using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class TypeOfUser : EntityBase
    {
        public TypeOfUser()
        {
            Users = new HashSet<User>();
            UserDecentralizations = new HashSet<UserDecentralization>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<UserDecentralization> UserDecentralizations { get; set; }
    }
}
