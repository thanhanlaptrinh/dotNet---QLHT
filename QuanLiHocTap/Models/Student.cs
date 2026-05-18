using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLiHocTap.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        [StringLength(20)]
        public string StudentCode { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
    }
}
