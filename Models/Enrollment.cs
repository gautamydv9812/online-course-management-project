using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourseManagementPortal.Models
{
    public class Enrollment
    {
        public Enrollment()
        {
            Grade = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Status")]
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        [Display(Name = "Grade")]
        [StringLength(2)]
        public string Grade { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }

    public enum EnrollmentStatus
    {
        Active,
        Completed,
        Dropped,
        Failed
    }
}