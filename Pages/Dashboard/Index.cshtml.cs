using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System.Collections.Generic;
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

        // Existing properties
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalEnrollments { get; set; }
        public int ActiveCourses { get; set; }
        public int ActiveStudents { get; set; }
        public int ActiveInstructors { get; set; }
        public int ActiveEnrollments { get; set; }

        // New property for mixed activities
        public List<ActivityItem> RecentActivities { get; set; } = new List<ActivityItem>();

        public async Task OnGetAsync()
        {
            // Existing dashboard stats
            TotalCourses = await _context.Courses.CountAsync();
            TotalStudents = await _context.Students.CountAsync();
            TotalInstructors = await _context.Instructors.CountAsync();
            TotalEnrollments = await _context.Enrollments.CountAsync();

            ActiveCourses = await _context.Courses.CountAsync(c => c.IsActive);
            ActiveStudents = await _context.Students.CountAsync(s => s.IsActive);
            ActiveInstructors = await _context.Instructors.CountAsync(i => i.IsActive);
            ActiveEnrollments = await _context.Enrollments
                .CountAsync(e => e.Status == EnrollmentStatus.Active);

            // Get recent activities mixed from all tables
            var activities = new List<ActivityItem>();

            // Recent enrollments
            var recentEnrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrollmentDate)
                .Take(5)
                .ToListAsync();

            foreach (var enrollment in recentEnrollments)
            {
                activities.Add(new ActivityItem
                {
                    Date = enrollment.EnrollmentDate,
                    Type = "Enrollment",
                    Description = $"{enrollment.Student?.FullName} enrolled in {enrollment.Course?.Title}"
                });
            }

            // Recent courses
            var recentCourses = await _context.Courses
                .OrderByDescending(c => c.StartDate)
                .Take(5)
                .ToListAsync();

            foreach (var course in recentCourses)
            {
                activities.Add(new ActivityItem
                {
                    Date = course.StartDate,
                    Type = "Course",
                    Description = $"New course '{course.Title}' added"
                });
            }

            // Recent students
            var recentStudents = await _context.Students
                .OrderByDescending(s => s.EnrollmentDate)
                .Take(5)
                .ToListAsync();

            foreach (var student in recentStudents)
            {
                activities.Add(new ActivityItem
                {
                    Date = student.EnrollmentDate,
                    Type = "Student",
                    Description = $"New student {student.FullName} registered"
                });
            }

            // Recent instructors
            var recentInstructors = await _context.Instructors
                .OrderByDescending(i => i.HireDate)
                .Take(5)
                .ToListAsync();

            foreach (var instructor in recentInstructors)
            {
                activities.Add(new ActivityItem
                {
                    Date = instructor.HireDate,
                    Type = "Instructor",
                    Description = $"New instructor {instructor.FullName} joined"
                });
            }

            // Sort all activities by date (newest first) and take top 5
            RecentActivities = activities
                .OrderByDescending(a => a.Date)
                .Take(5)
                .ToList();
        }
    }

    public class ActivityItem
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}