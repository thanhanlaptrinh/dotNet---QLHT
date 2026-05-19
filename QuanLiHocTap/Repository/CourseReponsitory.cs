using QuanLiHocTap.Data;
using System;
using System.Collections.Generic;
using System.Text;
using QuanLiHocTap.Helper;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace QuanLiHocTap.Repository
{
    public class CourseReponsitory
    {
        public ObservableCollection<GradeStatusModel> GetGradeStatus()
        {
            ObservableCollection<GradeStatusModel> gradeStatus = new ObservableCollection<GradeStatusModel>();
            string query = @"
                SELECT DISTINCT o.ClassCode + ' - ' + c.CourseName AS CourseClass, u.FullName AS Lecturer, g.GradeStatus
                FROM Grades g
                JOIN Enrollments e ON g.EnrollmentID = e.EnrollmentID
                JOIN Offerings o ON e.OfferingID = o.OfferingID
                JOIN Courses c ON o.CourseID = c.CourseID
                JOIN Users u ON o.LecturerID = u.UserID";
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gradeStatus.Add(new GradeStatusModel
                            {
                                CourseClass = reader["CourseClass"].ToString(),
                                Lecturer = reader["Lecturer"].ToString(),
                                Status = reader["GradeStatus"].ToString()
                            });
                        }
                        return gradeStatus;
                    }
                }
            }
        }

        public ObservableCollection<WorkflowStepModel> GetWorkflowStep()
        {
            ObservableCollection<WorkflowStepModel> workflowSteps = new ObservableCollection<WorkflowStepModel>();
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                var steps = new List<string> { "Draft", "Submitted", "Approved", "Published" };
                foreach (var step in steps)
                {
                    string queryWF = "SELECT COUNT(*) FROM Grades WHERE GradeStatus = @Status";
                    using (SqlCommand cmd = new SqlCommand(queryWF, con))
                    {
                        cmd.Parameters.AddWithValue("@Status", step);
                        int stepCount = Convert.ToInt32(cmd.ExecuteScalar());
                        workflowSteps.Add(new WorkflowStepModel { StepName = step, Count = stepCount });
                    }
                }
                return workflowSteps;
            }
        }
    }
}
