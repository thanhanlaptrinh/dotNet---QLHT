using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiHocTap.Data
{
    public class GradeStatusModel
    {
        public string CourseClass { get; set; }
        public string Lecturer { get; set; }
        public string Status { get; set; } 
    }

    public class RecentRegistrationModel
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
    }

    public class WorkflowStepModel
    {
        public string StepName { get; set; }
        public int Count { get; set; }  
    }
}
