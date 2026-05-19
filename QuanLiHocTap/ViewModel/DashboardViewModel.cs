using Microsoft.Data.SqlClient;
using QuanLiHocTap.Data;
using QuanLiHocTap.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QuanLiHocTap.ViewModel
{
    public class DashboardViewModel:BaseViewModel
    {
        private int _totalStudents;
        public int TotalStudents
        {
            get {  return _totalStudents; }
            set { 
                _totalStudents = value;
                OnPropertyChanged();
            }
        }
        private int _totalEnrollments;
        public int TotalEnrollments
        {
            get { return _totalEnrollments; }
            set
            {
                _totalEnrollments = value;
                OnPropertyChanged();
            }
        }
        private int _pendingGradesCount;
        public int PendingGradesCount
        {
            get { return _pendingGradesCount; }
            set
            {
                _pendingGradesCount = value;
                OnPropertyChanged();
            }
        }
        private int _reviewRequestsCount;
        public int ReviewRequestsCount
        {
            get { return _reviewRequestsCount; }
            set
            {
                _reviewRequestsCount = value;
                OnPropertyChanged();
            }
        }
        private int _registeredCredits;
        public int RegisteredCredits
        {
            get { return _registeredCredits; }
            set
            {
                _registeredCredits = value;
                OnPropertyChanged();
            }
        }

        //======================================List==============================================
        private ObservableCollection<GradeStatusModel> _gradeStatuses = new ObservableCollection<GradeStatusModel>();
        public ObservableCollection<GradeStatusModel> GradeStatuses
        {
            get { return _gradeStatuses; }
            set { 
                _gradeStatuses = value; 
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<RecentRegistrationModel> _recentRegistrations = new ObservableCollection<RecentRegistrationModel>();
        public ObservableCollection<RecentRegistrationModel> RecentRegistrations
        {
            get { return _recentRegistrations; }
            set { 
                _recentRegistrations = value; 
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<WorkflowStepModel> _workflowSteps = new ObservableCollection<WorkflowStepModel>();
        public ObservableCollection<WorkflowStepModel> WorkflowSteps
        {
            get { return _workflowSteps; }
            set { 
                _workflowSteps = value; 
                OnPropertyChanged(); 
            }
        }
        //======================================Contructor========================================
        public DashboardViewModel()
        {
            Load_Data();
        }

        //=======================================Method===========================================
        private void Load_Data()
        {
            try
            {
                TotalStudents = studentReponsitory.Count_Students();
                TotalEnrollments=enrollmentRepository.Count_Enrollment();
                PendingGradesCount = gradeReponsitory.GetPendingGradesCount();
                ReviewRequestsCount = recheckRepository.countRechecks();
                RegisteredCredits = enrollmentRepository.Count_Credits();

                GradeStatuses.Clear();
                GradeStatuses = courseReponsitory.GetGradeStatus();

                RecentRegistrations.Clear();
                RecentRegistrations = enrollmentRepository.GetRecent();

                WorkflowSteps.Clear();
                WorkflowSteps = courseReponsitory.GetWorkflowStep();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
