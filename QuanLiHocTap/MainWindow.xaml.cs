using Microsoft.CognitiveServices.Speech;
using QuanLiHocTap.Data;
using QuanLiHocTap.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiHocTap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            MainContent.Content = new DashboardView();
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        private void Nav_Dashboard(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Tổng quan hệ thống";
            txtPageDesc.Text = "Trang chủ";
            MainContent.Content = new DashboardView();
        }

        private void Nav_Enrollment(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Đăng ký học phần";
            txtPageDesc.Text = "Đăng ký học phần → Danh sách";
            MainContent.Content = new EnrollmentManagerView(); 
        }

        private void Nav_Recheck(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Yêu cầu phúc khảo";
            txtPageDesc.Text = "Học vụ → Phúc khảo";
            MainContent.Content = new RecheckRequestView(); 
        }

        private void Nav_GradeEntry(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Nhập điểm học viên";
            txtPageDesc.Text = "Điểm số → Nhập điểm";
            MainContent.Content = new GradeEntryView();
        }

        private void Nav_Approval(object sender, RoutedEventArgs e)
        {
            txtPageDesc.Text = "Duyệt điểm";
            txtPageTitle.Text = "Điểm số → Duyệt & Công bố";
             MainContent.Content = new GradeApprovalView(); //new ApprovalView();
        }

        private void Nav_Account(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Thông tin tài khoản";
            txtPageDesc.Text = "Tài khoản → Chi tiết tài khoản";
            MainContent.Content = new AccountView();
        }

        private void Nav_OfferingBrowser(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Lớp học mở";
            txtPageDesc.Text = "Đăng ký học phần → Lớp mở";
            MainContent.Content = new OfferingBrowserView();
        }

        private void Nav_Transcrip(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Bảng điểm học sinh";
            txtPageDesc.Text = "Học vụ → Transcript";
            MainContent.Content = new TranscriptView();
        }

        private void Nav_Statis(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Thống kê";
            txtPageDesc.Text = "Báo cáo → Statistics";
            MainContent.Content = new StatisticsView();
        }
    }
}