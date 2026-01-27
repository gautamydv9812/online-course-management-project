using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = new Course();

        public SelectList Instructors { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var instructors = await _context.Instructors
                .Where(i => i.IsActive)
                .OrderBy(i => i.LastName)
                .ToListAsync();

            Instructors = new SelectList(instructors, "Id", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var instructors = await _context.Instructors
                    .Where(i => i.IsActive)
                    .ToListAsync();
                Instructors = new SelectList(instructors, "Id", "FullName");
                return Page();
            }

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Course created successfully!";
            return RedirectToPage("./Index");
        }
    }
}