using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourseManagementPortal.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        public async Task OnGetAsync()
        {
            Instructors = await _context.Instructors
                .Include(i => i.Courses)
                .OrderBy(i => i.LastName)
                .ToListAsync();
        }
    }
}