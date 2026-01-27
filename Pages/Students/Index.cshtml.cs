using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Student> Students { get; set; } = new List<Student>();

        public async Task OnGetAsync()
        {
            Students = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .OrderBy(s => s.LastName)
                .ToListAsync();
        }
    }
}