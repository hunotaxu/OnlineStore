using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Event
{
    public class IndexModel : PageModel
    {
        public IList<DAL.Models.Event> Event { get; set; }

        public IndexModel()
        {
            
        }

        public void OnGet()
        {

        }
    }
}