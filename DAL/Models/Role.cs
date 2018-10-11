using DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class Role : EntityBase
    {
        public Role()
        {
            UserDecentralization = new HashSet<UserDecentralization>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserDecentralization> UserDecentralization { get; set; }
    }
}
