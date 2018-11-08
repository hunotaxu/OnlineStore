using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Event
{
    public class EditModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        public DAL.Models.Event Event { get; set; }

        public EditModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DAL.Models.Event evt = _eventRepository.Find(n => n.Id == id);
            if (evt.StartDate != null)
            {
                TempData["StartDate"] = evt.StartDate.ToString("MM/dd/yyyy");
            }

            if (evt.EndDate != null)
            {
                TempData["EndDate"] = evt.EndDate.ToString("MM/dd/yyyy");
            }
            return Page();
        }

        public void OnPost()
        {

        }
    }
}