using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class UserDecentralization
    {
        public int TypeOfUserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public TypeOfUser TypeOfUser { get; set; }
    }
}
