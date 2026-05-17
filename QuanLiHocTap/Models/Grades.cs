using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiHocTap.Models
{
    class Grades
    {
        private string gradeId;
        public string GradeId
        {
            get { return gradeId; }
            set { gradeId = value; }
        }
        private string enrollmentId;
        public string EnrollmentId
        {
            get { return enrollmentId; }
            set { enrollmentId = value; }
        }
        private string componentId;
        public string ComponentId
        {
            get { return componentId; }
            set { componentId = value; }
        }
        private float score;
        public float Score
        {
            get { return score; }
            set { score = value; }
        }
        private string gradeStatus;
        public string GradeStatus
        {
            get { return gradeStatus; }
            set { gradeStatus = value; }
        }
    }
}
