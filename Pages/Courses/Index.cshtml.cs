using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Course> Courses { get; set; } = new List<Course>();

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Instructor)
                .OrderBy(c => c.Code)
                .ToListAsync();
        }
    }
}