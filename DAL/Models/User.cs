using DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class User : EntityBase
    {
        public User()
        {
            Cart = new HashSet<Cart>();
            Order = new HashSet<Order>();
        }

        //public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? TypeOfUserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Status { get; set; }
        //public byte[] Timestamp { get; set; }

        public TypeOfUser TypeOfUser { get; set; }
        public ICollection<Cart> Cart { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
