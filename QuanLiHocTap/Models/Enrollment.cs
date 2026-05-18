using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLiHocTap.Models
{
    [Table("Enrollments")]
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int OfferingId { get; set; }
        [StringLength(50)]
        public string EnrollStatus { get; set; }
        public DateTime? EnrolledAt { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("OfferingId")]
        public virtual Offering Offering { get; set; }
    }
}
