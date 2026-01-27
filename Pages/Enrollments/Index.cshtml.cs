using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public async Task OnGetAsync()
        {
            Enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .OrderByDescending(e => e.EnrollmentDate)
                .ToListAsync();
        }
    }
}