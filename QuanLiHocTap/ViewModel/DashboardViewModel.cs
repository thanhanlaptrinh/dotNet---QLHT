using QuanLiHocTap.Repository;
using System;
using System.Collections.Generic;
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

        //======================================Contructor========================================
        public DashboardViewModel()
        {
            Load_Data();
        }

        private void Load_Data()
        {
            try
            {
                StudentRepository studentReponsitory = new StudentRepository();
                TotalStudents = studentReponsitory.Count_Students();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
