using DAL.Data.Enums;

namespace DAL.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
