using QuanLyPhongTro.Data;
using QuanLyPhongTro.Repositories;
using QuanLyPhongTro.Services;
using QuanLyPhongTro.ViewModels;
using QuanLyPhongTro.Views;
using System.Windows;

namespace QuanLyPhongTro
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new AppDbContext();

            var userRepository = new UserRepository(dbContext);
            var roomRepository = new RoomRepository(dbContext);
            var tenantRepository = new TenantRepository(dbContext);
            var contractRepository = new ContractRepository(dbContext);
            var occupantRepository = new OccupantRepository(dbContext);
            var invoiceRepository = new InvoiceRepository(dbContext);
            var meterReadingRepository = new MeterReadingRepository(dbContext);

            var userService = new UserService(userRepository);
            var roomService = new RoomService(roomRepository);
            var tenantService = new TenantService(tenantRepository);
            var occupantService = new OccupantService(occupantRepository, contractRepository);
            var contractService = new ContractService(contractRepository, roomRepository, tenantRepository);
            var invoiceService = new InvoiceService(invoiceRepository, roomRepository, meterReadingRepository);
            var meterReadingService = new MeterReadingService(meterReadingRepository, roomRepository, invoiceRepository);

            var loginViewModel = new LoginViewModel(userService);
            var loginWindow = new LoginWindow();
            loginWindow.DataContext = loginViewModel;

            loginViewModel.OnLoginSuccess += (loggedInUser) =>
            {
                var mainViewModel = new MainViewModel(
                    roomService,
                    tenantService,
                    contractService,
                    invoiceService,
                    occupantService,
                    meterReadingService,
                    loggedInUser);

                var mainWindow = new MainWindow(mainViewModel);

                loginWindow.Hide(); 
                mainWindow.Show(); 

                mainViewModel.OnLogoutRequested += () =>
                {
                    loginViewModel.Password = string.Empty;
                    loginWindow.Show();
                };
            };

            loginWindow.Show();
        }
    }
}