using System.Collections.Generic;

namespace Utilities.DTOs
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public PagedResult()
        {
            Results = new List<T>();
            All = new List<T>();
        }
        public IList<T> Results { get; set; }
        public IList<T> All { get; set; }
    }
}
