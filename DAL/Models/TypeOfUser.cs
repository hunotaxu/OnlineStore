using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class TypeOfUser : EntityBase
    {
        public TypeOfUser()
        {
            User = new HashSet<User>();
            UserDecentralization = new HashSet<UserDecentralization>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<User> User { get; set; }
        public ICollection<UserDecentralization> UserDecentralization { get; set; }
    }
}
