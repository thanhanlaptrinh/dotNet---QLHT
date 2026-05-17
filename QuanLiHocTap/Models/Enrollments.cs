using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiHocTap.Models
{
    class Enrollments
    {
        private string enrollmentId;
        public string EnrollmentId
        {
            get { return enrollmentId; }
            set { enrollmentId = value; }
        }
        private string studentId;
        public string StudentId
        {
            get { return studentId; }
            set { studentId = value; }
        }
        private string offeringId;
        public string OfferingId
        {
            get { return offeringId; }
            set { offeringId = value; }
        }
        private string enrollStatus;
        public string EnrollStatus
        {
            get { return enrollStatus; }
            set { enrollStatus = value; }
        }
        private string enrolledAt;
        public string EnrolledAt
        {
            get { return enrolledAt; }
            set { enrolledAt = value; }
        }

    }
}
