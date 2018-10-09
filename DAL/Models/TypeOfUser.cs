using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class TypeOfUser
    {
        public TypeOfUser()
        {
            User = new HashSet<User>();
            UserDecentralization = new HashSet<UserDecentralization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Timestamp { get; set; }

        public ICollection<User> User { get; set; }
        public ICollection<UserDecentralization> UserDecentralization { get; set; }
    }
}
