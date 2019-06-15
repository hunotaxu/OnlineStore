using DAL.Data.Entities.Base;
using System;

namespace DAL.Data.Entities
{
    public class ShowRoomAddress : EntityBase
    {
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Detail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual Province Province { get; set; }
        public virtual Ward Ward { get; set; }
        public virtual District District { get; set; }
    }
}
