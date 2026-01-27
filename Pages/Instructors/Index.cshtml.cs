using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Collections.Generic;
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

        public IList<Instructor> Instructor { get; set; } = new List<Instructor>();

        public async Task OnGetAsync()
        {
            Instructor = await _context.Instructors
                .Include(i => i.Courses)
                .ToListAsync();
        }
    }
}
