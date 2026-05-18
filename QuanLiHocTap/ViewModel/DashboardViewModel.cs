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
        private int _studentRepository;
        public int StudentRepository
        {
            get { return _studentRepository; }
            set
            {
                _studentRepository = value;
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
                StudentRepository studentReponsitory = new StudentRepository();
                TotalStudents = studentReponsitory.Count_Students();
                EnrollmentRepository enrollmentRepository = new EnrollmentRepository();
                TotalEnrollments=enrollmentRepository.Count_Enrollment();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
