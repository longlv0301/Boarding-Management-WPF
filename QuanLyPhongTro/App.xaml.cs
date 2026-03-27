using QuanLyPhongTro.Data;
using QuanLyPhongTro.Repositories;
using QuanLyPhongTro.Services;
using QuanLyPhongTro.ViewModels;
using QuanLyPhongTro.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace QuanLyPhongTro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App   
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new AppDbContext();
            var userRepository = new UserRepository(dbContext);

            var userService = new UserService(userRepository);

            var loginViewModel = new LoginViewModel(userService);

            var loginWindow = new LoginWindow();
            loginWindow.DataContext = loginViewModel;

            loginWindow.Show();
        }
    }

}
