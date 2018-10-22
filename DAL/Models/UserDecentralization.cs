namespace DAL.Models
{
    public class UserDecentralization
    {
        public int TypeOfUserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public TypeOfUser TypeOfUser { get; set; }
    }
}
