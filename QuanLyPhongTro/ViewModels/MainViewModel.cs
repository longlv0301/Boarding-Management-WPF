using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.Commands;
using QuanLyPhongTro.Data;
using QuanLyPhongTro.Models;
using QuanLyPhongTro.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyPhongTro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Biến quản lý màn hình hiển thị
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        // Thông tin người dùng đang đăng nhập
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; OnPropertyChanged(); }
        }

        private readonly IRoomService _roomService;
        private readonly ITenantService _tenantService;
        private readonly IContractService _contractService;
        private readonly IInvoiceService _invoiceService;
        private readonly IOccupantService _occupantService;
        private readonly IMeterReadingService _meterReadingService;

        public ICommand ShowRoomCommand { get; }
        public ICommand ShowTenantCommand { get; }
        public ICommand ShowContractCommand { get; }
        public ICommand ShowInvoiceCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ShowMeterReadingCommand { get; }

        public event Action OnLogoutRequested;

        public MainViewModel( 
            IRoomService roomService,
            ITenantService tenantService,
            IContractService contractService,
            IInvoiceService invoiceService,
            IOccupantService occupantService,
            IMeterReadingService meterReadingService,
            User loggedInUser)
        {
            _roomService = roomService;
            _tenantService = tenantService;
            _contractService = contractService;
            _invoiceService = invoiceService;
            _occupantService = occupantService;
            _meterReadingService = meterReadingService;

            CurrentUser = loggedInUser;

            ShowRoomCommand = new RelayCommand(ExecuteShowRoom);
            ShowTenantCommand = new RelayCommand(ExecuteShowTenant);
            ShowContractCommand = new RelayCommand(ExecuteShowContract);
            ShowInvoiceCommand = new RelayCommand(ExecuteShowInvoice);
            ShowMeterReadingCommand = new RelayCommand(ExecuteShowMeterReading);

            LogoutCommand = new RelayCommand(ExecuteLogout);

            ExecuteShowRoom(null);
        }

        private void ExecuteShowRoom(object o)
        {
            CurrentViewModel = new RoomViewModel(_roomService);
        }

        private void ExecuteShowTenant(object obj)
        {
            CurrentViewModel = new TenantViewModel(_tenantService);
        }

        private void ExecuteShowContract(object obj)
        {
            CurrentViewModel = new ContractViewModel(_contractService, _roomService, _tenantService, _occupantService);
        }

        private void ExecuteShowInvoice(object obj)
        {
            CurrentViewModel = new InvoiceViewModel(_invoiceService, _roomService);
        }

        private void ExecuteShowMeterReading(object obj)
        {
            // Đảm bảo bạn truyền đủ các Service mà MeterReadingViewModel cần
            CurrentViewModel = new MeterReadingViewModel(_meterReadingService, _roomService);
        }

        // --- 7. LOGIC ĐĂNG XUẤT ---
        private void ExecuteLogout(object obj)
        {
            OnLogoutRequested?.Invoke();
        }

    }
}
