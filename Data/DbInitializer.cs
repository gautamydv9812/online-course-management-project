using OnlineCourseManagementPortal.Models;
using System;
using System.Linq;

namespace OnlineCourseManagementPortal.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return;
            }

            var instructors = new Instructor[]
            {
                new Instructor
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@university.edu",
                    Phone = "123-456-7890",
                    DateOfBirth = DateTime.Parse("1980-01-15"),
                    HireDate = DateTime.Parse("2010-09-01"),
                    Specialization = "Computer Science",
                    Salary = 75000,
                    IsActive = true
                },
                new Instructor
                {
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "sarah.johnson@university.edu",
                    Phone = "123-456-7891",
                    DateOfBirth = DateTime.Parse("1975-03-22"),
                    HireDate = DateTime.Parse("2012-01-15"),
                    Specialization = "Mathematics",
                    Salary = 72000,
                    IsActive = true
                },
                new Instructor
                {
                    FirstName = "Michael",
                    LastName = "Brown",
                    Email = "michael.brown@university.edu",
                    Phone = "123-456-7892",
                    DateOfBirth = DateTime.Parse("1985-08-10"),
                    HireDate = DateTime.Parse("2015-03-01"),
                    Specialization = "Physics",
                    Salary = 68000,
                    IsActive = true
                }
            };

            context.Instructors.AddRange(instructors);
            context.SaveChanges();

            var courses = new Course[]
            {
                new Course
                {
                    Title = "Introduction to Programming",
                    Description = "Learn basic programming concepts with C#",
                    Code = "CS101",
                    CreditHours = 3,
                    Price = 299.99m,
                    StartDate = DateTime.Parse("2024-01-15"),
                    EndDate = DateTime.Parse("2024-05-15"),
                    MaxStudents = 30,
                    IsActive = true,
                    InstructorId = instructors[0].Id
                },
                new Course
                {
                    Title = "Web Development with ASP.NET",
                    Description = "Build web applications using ASP.NET Core",
                    Code = "CS202",
                    CreditHours = 4,
                    Price = 399.99m,
                    StartDate = DateTime.Parse("2024-02-01"),
                    EndDate = DateTime.Parse("2024-06-01"),
                    MaxStudents = 25,
                    IsActive = true,
                    InstructorId = instructors[0].Id
                },
                new Course
                {
                    Title = "Calculus I",
                    Description = "Introduction to differential and integral calculus",
                    Code = "MATH101",
                    CreditHours = 4,
                    Price = 349.99m,
                    StartDate = DateTime.Parse("2024-01-20"),
                    EndDate = DateTime.Parse("2024-05-20"),
                    MaxStudents = 35,
                    IsActive = true,
                    InstructorId = instructors[1].Id
                },
                new Course
                {
                    Title = "Database Management",
                    Description = "Learn SQL and database design principles",
                    Code = "CS301",
                    CreditHours = 3,
                    Price = 349.99m,
                    StartDate = DateTime.Parse("2024-03-01"),
                    EndDate = DateTime.Parse("2024-07-01"),
                    MaxStudents = 28,
                    IsActive = true,
                    InstructorId = instructors[2].Id
                }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();

            var students = new Student[]
            {
                new Student
                {
                    FirstName = "Sambhav",
                    LastName = "Silwal",
                    Email = "sambhav.silwal@student.edu",
                    Phone = "9841000001",
                    DateOfBirth = DateTime.Parse("2000-05-10"),
                    EnrollmentDate = DateTime.Parse("2023-09-01"),
                    Address = "Kathmandu",
                    City = "Kathmandu",
                    Country = "Nepal",
                    IsActive = true
                },
                new Student
                {
                    FirstName = "Asmi",
                    LastName = "Pandey",
                    Email = "asmi.pandey@student.edu",
                    Phone = "9841000002",
                    DateOfBirth = DateTime.Parse("2001-07-15"),
                    EnrollmentDate = DateTime.Parse("2023-09-01"),
                    Address = "Pokhara",
                    City = "Pokhara",
                    Country = "Nepal",
                    IsActive = true
                },
                new Student
                {
                    FirstName = "Gautam",
                    LastName = "Kumar Yadav",
                    Email = "gautam.yadav@student.edu",
                    Phone = "9841000003",
                    DateOfBirth = DateTime.Parse("1999-12-20"),
                    EnrollmentDate = DateTime.Parse("2023-09-01"),
                    Address = "Biratnagar",
                    City = "Biratnagar",
                    Country = "Nepal",
                    IsActive = true
                },
                new Student
                {
                    FirstName = "Sejal",
                    LastName = "Oli",
                    Email = "sejal.oli@student.edu",
                    Phone = "9841000004",
                    DateOfBirth = DateTime.Parse("2000-03-25"),
                    EnrollmentDate = DateTime.Parse("2023-09-01"),
                    Address = "Dharan",
                    City = "Dharan",
                    Country = "Nepal",
                    IsActive = true
                },
                new Student
                {
                    FirstName = "Rahul",
                    LastName = "Sharma",
                    Email = "rahul.sharma@student.edu",
                    Phone = "9841000005",
                    DateOfBirth = DateTime.Parse("2000-08-30"),
                    EnrollmentDate = DateTime.Parse("2023-09-01"),
                    Address = "Lalitpur",
                    City = "Lalitpur",
                    Country = "Nepal",
                    IsActive = true
                }
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = courses[0].Id,
                    EnrollmentDate = DateTime.Parse("2024-01-10"),
                    Status = EnrollmentStatus.Active,
                    Grade = "A"
                },
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = courses[1].Id,
                    EnrollmentDate = DateTime.Parse("2024-01-15"),
                    Status = EnrollmentStatus.Active,
                    Grade = "B+"
                },
                new Enrollment
                {
                    StudentId = students[1].Id,
                    CourseId = courses[0].Id,
                    EnrollmentDate = DateTime.Parse("2024-01-10"),
                    Status = EnrollmentStatus.Active,
                    Grade = "A-"
                },
                new Enrollment
                {
                    StudentId = students[2].Id,
                    CourseId = courses[2].Id,
                    EnrollmentDate = DateTime.Parse("2024-01-20"),
                    Status = EnrollmentStatus.Active,
                    Grade = "B"
                },
                new Enrollment
                {
                    StudentId = students[3].Id,
                    CourseId = courses[1].Id,
                    EnrollmentDate = DateTime.Parse("2024-01-15"),
                    Status = EnrollmentStatus.Active,
                    Grade = "A"
                },
                new Enrollment
                {
                    StudentId = students[4].Id,
                    CourseId = courses[3].Id,
                    EnrollmentDate = DateTime.Parse("2024-02-01"),
                    Status = EnrollmentStatus.Active,
                    Grade = "A+"
                },
                new Enrollment
                {
                    StudentId = students[1].Id,
                    CourseId = courses[3].Id,
                    EnrollmentDate = DateTime.Parse("2024-02-01"),
                    Status = EnrollmentStatus.Active,
                    Grade = "B+"
                }
            };

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}