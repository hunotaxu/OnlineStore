using System.Collections.Generic;

namespace Utilities.DTOs
{
    public class PagedResult<T> : PagedResultBase where T:class
    {
        public PagedResult()
        {
            Results = new List<T>();
        }
        public IList<T> Results { get; set; }
    }
}
