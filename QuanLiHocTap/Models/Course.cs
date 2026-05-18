using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLiHocTap.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [StringLength(20)]
        public string CourseCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }
        public int Credits { get; set; }
    }
}
