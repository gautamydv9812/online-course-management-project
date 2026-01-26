using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalEnrollments { get; set; }
        public int ActiveCourses { get; set; }
        public int ActiveStudents { get; set; }
        public int ActiveInstructors { get; set; }
        public int ActiveEnrollments { get; set; }

        public async Task OnGetAsync()
        {
            TotalCourses = await _context.Courses.CountAsync();
            TotalStudents = await _context.Students.CountAsync();
            TotalInstructors = await _context.Instructors.CountAsync();
            TotalEnrollments = await _context.Enrollments.CountAsync();

            ActiveCourses = await _context.Courses.CountAsync(c => c.IsActive);
            ActiveStudents = await _context.Students.CountAsync(s => s.IsActive);
            ActiveInstructors = await _context.Instructors.CountAsync(i => i.IsActive);
            ActiveEnrollments = await _context.Enrollments
                .CountAsync(e => e.Status == EnrollmentStatus.Active);
        }
    }
}