using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourseManagementPortal.Models
{
    public class Course
    {
        public Course()
        {
            Title = string.Empty;
            Description = string.Empty;
            Code = string.Empty;
            Enrollments = new List<Enrollment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Course Title")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Course Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Credit Hours")]
        [Range(1, 6)]
        public int CreditHours { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Maximum Students")]
        [Range(1, 100)]
        public int MaxStudents { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        [NotMapped]
        [Display(Name = "Current Students")]
        public int CurrentStudents => Enrollments?.Count ?? 0;

        [NotMapped]
        [Display(Name = "Available Slots")]
        public int AvailableSlots => MaxStudents - CurrentStudents;
    }
}