using QuanLyPhongTro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyPhongTro.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem giao diện này đã được gắn LoginViewModel chưa
            if (this.DataContext is LoginViewModel vm)
            {
                // Ép mật khẩu từ XAML đẩy thẳng xuống biến Password của ViewModel
                vm.Password = txtPassword.Password;
            }
        }
    }
}
