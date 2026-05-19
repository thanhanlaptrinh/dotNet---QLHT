using QuanLiHocTap.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuanLiHocTap.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StudentRepository studentReponsitory = new StudentRepository();
        public EnrollmentRepository enrollmentRepository = new EnrollmentRepository();
        public GradeReponsitory gradeReponsitory =new GradeReponsitory();
        public RecheckRepository recheckRepository = new RecheckRepository();
        public CourseReponsitory courseReponsitory = new CourseReponsitory();

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
