using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Instructors
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Instructor = await _context.Instructors.FindAsync(id);

            if (Instructor == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var instructor = await _context.Instructors.FindAsync(Instructor.Id);

            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
