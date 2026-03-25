using QuanLyPhongTro.Commands;
using QuanLyPhongTro.Models;
using QuanLyPhongTro.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyPhongTro.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        private bool _isFirstRun;
        public bool IsFirstRun
        {
            get { return _isFirstRun; }
            set
            {
                _isFirstRun = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLoginVisible));
            }
        }

        public bool IsLoginVisible => !IsFirstRun;

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _fullName; 
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand CreateAdminCommand { get; }

        public event Action<User> OnLoginSuccess;

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;

            IsFirstRun = _userService.IsFirstRun();

            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            CreateAdminCommand = new RelayCommand(ExecuteCreateAdmin, CanExecuteCreateAdmin);
        }

        private bool CanExecuteLogin(object obj)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteLogin(object obj)
        {
            var user = _userService.Authenticate(Username, Password, out string errorMessage);

            if (user != null)
            {
                OnLoginSuccess?.Invoke(user);
            }
            else
            {
                MessageBox.Show(errorMessage, "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteCreateAdmin(object obj)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(FullName);
        }

        private void ExecuteCreateAdmin(object obj)
        {
            bool success = _userService.CreateInitialAdmin(Username, Password, FullName, out string errorMessage);

            if (success)
            {
                MessageBox.Show("Khởi tạo tài khoản Quản trị thành công! Vui lòng đăng nhập lại.", "Thông báo");

                IsFirstRun = false;
                Password = string.Empty; 
            }
            else
            {
                MessageBox.Show(errorMessage, "Lỗi tạo tài khoản", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
