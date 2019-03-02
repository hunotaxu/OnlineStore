using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public enum StatusOrder
    {
        Pending,
        ReadyToShip,
        Shipped,
        Delivered,
        Canceled,
        WaitingToReturn,
        Returned
    }
}
