using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; } = new Enrollment();

        public SelectList Students { get; set; }
        public SelectList Courses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var students = await _context.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ToListAsync();

            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.IsActive && c.StartDate <= DateTime.Today && c.EndDate >= DateTime.Today)
                .OrderBy(c => c.Title)
                .ToListAsync();

            Students = new SelectList(students, "Id", "FullName");
            Courses = new SelectList(courses, "Id", "TitleWithCode");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return Page();
            }

            // Check if student is already enrolled in this course
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == Enrollment.StudentId && e.CourseId == Enrollment.CourseId);

            if (existingEnrollment != null)
            {
                ModelState.AddModelError("", "This student is already enrolled in this course.");
                await LoadSelectLists();
                return Page();
            }

            // Check if course has available slots
            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == Enrollment.CourseId);

            if (course != null && course.CurrentStudents >= course.MaxStudents)
            {
                ModelState.AddModelError("", "This course is already full. No available slots.");
                await LoadSelectLists();
                return Page();
            }

            Enrollment.EnrollmentDate = DateTime.Today;
            _context.Enrollments.Add(Enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student enrolled successfully!";
            return RedirectToPage("./Index");
        }

        private async Task LoadSelectLists()
        {
            var students = await _context.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ToListAsync();

            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Title)
                .ToListAsync();

            Students = new SelectList(students, "Id", "FullName");
            Courses = new SelectList(courses, "Id", "TitleWithCode");
        }
    }
}