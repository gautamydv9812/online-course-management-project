using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Instructors
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Instructor Instructor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Instructor == null)
                return NotFound();

            return Page();
        }
    }
}