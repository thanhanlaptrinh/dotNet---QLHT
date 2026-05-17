using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiHocTap.Models
{
    class GradeComponents
    {
        private string componentId;
        public string ComponentId
        {
            get { return componentId; }
            set { componentId = value; }
        }
        private string courseId;
        public string CourseId
        {
            get { return courseId; }
            set { courseId = value; }
        }
        private string componentName;
        public string ComponentName
        { 
            get { return componentName; }
            set { componentName = value; }
        }
        private float weight;
        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        private float maxScore;
        public float MaxScore
        {
            get { return maxScore; }
            set { maxScore = value; }
        }
    }
}
