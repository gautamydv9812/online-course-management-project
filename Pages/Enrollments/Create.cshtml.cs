using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagementPortal.Data;
using OnlineCourseManagementPortal.Models;
using System;
using System.Collections.Generic;
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
            try
            {
                await LoadSelectLists();
                return Page();
            }
            catch (Exception ex)
            {
                // Initialize empty lists to prevent null reference
                Students = new SelectList(new List<Student>(), "Id", "FullName");
                Courses = new SelectList(new List<Course>(), "Id", "Title");

                ModelState.AddModelError("", $"Error loading data: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return Page();
            }

            try
            {
                // Check if student exists
                var studentExists = await _context.Students
                    .AnyAsync(s => s.Id == Enrollment.StudentId && s.IsActive);

                if (!studentExists)
                {
                    ModelState.AddModelError("Enrollment.StudentId", "Selected student does not exist or is not active.");
                    await LoadSelectLists();
                    return Page();
                }

                // Check if course exists
                var courseExists = await _context.Courses
                    .AnyAsync(c => c.Id == Enrollment.CourseId && c.IsActive);

                if (!courseExists)
                {
                    ModelState.AddModelError("Enrollment.CourseId", "Selected course does not exist or is not active.");
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

                if (course == null)
                {
                    ModelState.AddModelError("Enrollment.CourseId", "Selected course does not exist. Please select a valid course.");
                    await LoadSelectLists();
                    return Page();
                }

                if (course.CurrentStudents >= course.MaxStudents)
                {
                    ModelState.AddModelError("", "This course is already full. No available slots.");
                    await LoadSelectLists();
                    return Page();
                }

                // Set enrollment date if not provided
                if (Enrollment.EnrollmentDate == default)
                {
                    Enrollment.EnrollmentDate = DateTime.Today;
                }

                _context.Enrollments.Add(Enrollment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Student enrolled successfully!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                // Check for foreign key constraint violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN KEY constraint"))
                {
                    ModelState.AddModelError("", "Invalid student or course selected. Please ensure you select valid options from the dropdown.");
                }
                else
                {
                    ModelState.AddModelError("", $"Database error: {ex.InnerException?.Message ?? ex.Message}");
                }

                await LoadSelectLists();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                await LoadSelectLists();
                return Page();
            }
        }

        private async Task LoadSelectLists()
        {
            try
            {
                // Load students - get all data first, then filter in memory
                var studentsList = await _context.Students
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ToListAsync();

                // Filter for non-null FullName in memory (after ToListAsync)
                var validStudents = studentsList
                    .Where(s => !string.IsNullOrEmpty(s.FirstName) || !string.IsNullOrEmpty(s.LastName))
                    .Select(s => new
                    {
                        Id = s.Id,
                        // Create FullName from FirstName and LastName
                        FullName = $"{s.FirstName} {s.LastName}".Trim()
                    })
                    .ToList();

                Students = validStudents.Any()
                    ? new SelectList(validStudents, "Id", "FullName")
                    : new SelectList(new List<object>(), "Id", "FullName");

                // Load courses - get all data first
                var coursesList = await _context.Courses
                    .Include(c => c.Instructor)
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Title)
                    .ToListAsync();

                // Filter for current courses in memory
                var currentCourses = coursesList
                    .Where(c => c.StartDate <= DateTime.Today && c.EndDate >= DateTime.Today)
                    .ToList();

                // Use current courses if available, otherwise all active courses
                var finalCourses = currentCourses.Any()
                    ? currentCourses
                    : coursesList;

                // Create display items for courses
                var courseItems = finalCourses
                    .Where(c => !string.IsNullOrEmpty(c.Title))
                    .Select(c => new
                    {
                        Id = c.Id,
                        // Check if TitleWithCode property exists, otherwise use Title
                        DisplayName = GetCourseDisplayName(c)
                    })
                    .ToList();

                Courses = courseItems.Any()
                    ? new SelectList(courseItems, "Id", "DisplayName")
                    : new SelectList(new List<object>(), "Id", "DisplayName");

                // If no courses available, add a warning
                if (!finalCourses.Any())
                {
                    ModelState.AddModelError("", "No active courses available for enrollment. Please add courses first.");
                }
            }
            catch (Exception ex)
            {
                // If database error, create empty lists
                Students = new SelectList(new List<object>(), "Id", "FullName");
                Courses = new SelectList(new List<object>(), "Id", "DisplayName");
                ModelState.AddModelError("", $"Error loading dropdown data: {ex.Message}");
            }
        }

        private string GetCourseDisplayName(Course course)
        {
            if (course == null) return string.Empty;

            // Try to use TitleWithCode if the property exists and has value
            // Otherwise, create a display name from available properties
            var titleWithCodeProp = course.GetType().GetProperty("TitleWithCode");
            if (titleWithCodeProp != null)
            {
                var titleWithCodeValue = titleWithCodeProp.GetValue(course) as string;
                if (!string.IsNullOrEmpty(titleWithCodeValue))
                {
                    return titleWithCodeValue;
                }
            }

            // If we have Code property, use it with Title
            var codeProp = course.GetType().GetProperty("Code");
            if (codeProp != null)
            {
                var codeValue = codeProp.GetValue(course) as string;
                if (!string.IsNullOrEmpty(codeValue))
                {
                    return $"{course.Title} ({codeValue})";
                }
            }

            // Just use Title as fallback
            return course.Title ?? string.Empty;
        }
    }
}