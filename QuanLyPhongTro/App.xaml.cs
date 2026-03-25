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
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Khởi tạo tầng Database (DbContext)
            var dbContext = new AppDbContext();

            // 2. Khởi tạo tầng Repository (DAL)
            var roomRepository = new RoomRepository(dbContext);

            // 3. Khởi tạo tầng Service (BLL)
            var roomService = new RoomService(roomRepository);

            // 4. Khởi tạo tầng ViewModel và bơm Service vào
            var roomViewModel = new RoomViewModel(roomService);

            // 5. Khởi tạo Giao diện (View) và bơm ViewModel vào
            var roomWindow = new RoomWindow(roomViewModel);

            // 6. Ra lệnh hiển thị cửa sổ lên màn hình
            roomWindow.Show();
        }
    }

}
