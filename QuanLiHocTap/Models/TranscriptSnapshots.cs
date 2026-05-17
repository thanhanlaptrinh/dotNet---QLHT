using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiHocTap.Models
{
    class TranscriptSnapshots
    {
        private string snapshotId;
        public string SnapshotId
        {
            get { return snapshotId; }
            set { snapshotId = value; }
        }
        private string studentId;
        public string StudentId
        {
            get { return studentId; }
            set { studentId = value; }
        }
        private string semesterId;
        public string SemesterId
        {
            get { return semesterId; }
            set { semesterId = value; }
        }
        private float gpa;
        public float Gpa
        {
            get { return gpa; }
            set { gpa = value; }
        }
        private string totalCredits;
        public string TotalCredits
        {
            get { return totalCredits; }
            set { totalCredits = value; }
        }
    }
}
