using DAL.Data.Enums;

namespace DAL.Data.Interfaces
{
    public interface ISwitchable
    {
        byte Status { set; get; }
    }
}
