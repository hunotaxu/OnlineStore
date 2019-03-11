using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Entities
{
    public class UserDecentralization
    {
        public int TypeOfUserId { get; set; }
        public int RoleId { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Role Role { get; set; }
        public TypeOfUser TypeOfUser { get; set; }
    }
}
