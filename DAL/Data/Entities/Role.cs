//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using DAL.Data.Entities.Base;
//using DAL.Models;

//namespace DAL.Data.Entities
//{
//    public class Role : EntityBase
//    {
//        public Role()
//        {
//            UserDecentralizations = new HashSet<UserDecentralization>();
//        }

//        //public int Id { get; set; }
//        [Column(TypeName="nvarchar(200)")]
//        public string Name { get; set; }

//        [Column(TypeName = "nvarchar(500)")]
//        public string Description { get; set; }

//        public virtual ICollection<UserDecentralization> UserDecentralizations { get; set; }
//    }
//}
