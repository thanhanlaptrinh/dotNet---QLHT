using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLiHocTap.Models
{
    [Table("Grades")]
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public int EnrollmentId { get; set; }
        public int ComponentId { get; set; }
        public decimal? Score { get; set; }
        [StringLength(50)]
        public string GradeStatus { get; set; } // Draft -> Submitted -> Approved -> Published
        [StringLength(500)]
        public string RecheckNote { get; set; }
        public bool IsRechecked { get; set; }

        [ForeignKey("EnrollmentId")]
        public virtual Enrollment Enrollment { get; set; }
    }
}
