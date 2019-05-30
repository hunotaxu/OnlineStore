using System.Collections.Generic;
using System.Threading.Tasks;
using TimiApp.Dapper.ViewModels;

namespace TimiApp.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate);
    }
}
