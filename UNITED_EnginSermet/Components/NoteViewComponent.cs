using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNITED_EnginSermet.Data;
using UNITED_EnginSermet.DTOs;

namespace UNITED_EnginSermet.Components
{
    public class NoteViewComponent : ViewComponent
    {
        private readonly DataContext _context;

        public NoteViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(NoteDTO note)
        {
            return View(note);
        }
    }
}
