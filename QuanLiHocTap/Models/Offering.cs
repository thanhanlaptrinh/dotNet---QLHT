using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuanLiHocTap.Models
{
    [Table("Offerings")]
    public class Offering
    {
        [Key]
        public int OfferingId { get; set; }
        public int CourseId { get; set; }
        public int SemesterId { get; set; }
        [Required]
        [StringLength(20)]
        public string ClassCode { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
