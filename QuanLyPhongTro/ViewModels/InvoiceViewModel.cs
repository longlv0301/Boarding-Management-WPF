using QuanLyPhongTro.Commands;
using QuanLyPhongTro.Models;
using QuanLyPhongTro.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyPhongTro.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {

        private readonly IInvoiceService _invoiceService;
        private readonly IRoomService _roomService;

        private ObservableCollection<Room> _rentedRooms;
        public ObservableCollection<Room> RentedRooms
        {
            get { return _rentedRooms; }
            set { _rentedRooms = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Invoice> _invoices;
        public ObservableCollection<Invoice> Invoices
        {
            get { return _invoices; }
            set { _invoices = value; OnPropertyChanged(); }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set { _selectedRoom = value; OnPropertyChanged(); }
        }

        private int _selectedMonth = DateTime.Now.Month;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set { _selectedMonth = value; OnPropertyChanged(); }
        }

        private int _selectedYear = DateTime.Now.Year;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; OnPropertyChanged(); }
        }

        private decimal _electricPrice = 3500; // Đơn giá mặc định
        public decimal ElectricPrice
        {
            get { return _electricPrice; }
            set { _electricPrice = value; OnPropertyChanged(); }
        }

        private decimal _waterPrice = 20000; // Đơn giá mặc định
        public decimal WaterPrice
        {
            get { return _waterPrice; }
            set { _waterPrice = value; OnPropertyChanged(); }
        }

        private decimal _otherFees = 0; // Phụ phí (Rác, Wifi...)
        public decimal OtherFees
        {
            get { return _otherFees; }
            set { _otherFees = value; OnPropertyChanged(); }
        }

        // BIẾN LƯU HÓA ĐƠN ĐANG ĐƯỢC CHỌN TRÊN BẢNG (Để thanh toán)
        private Invoice _selectedInvoice;
        public Invoice SelectedInvoice
        {
            get { return _selectedInvoice; }
            set { _selectedInvoice = value; OnPropertyChanged(); }
        }

        public ICommand CreateInvoiceCommand { get; }
        public ICommand PayInvoiceCommand { get; }

        public InvoiceViewModel(IInvoiceService invoiceService, IRoomService roomService)
        {
            _invoiceService = invoiceService;
            _roomService = roomService;

            CreateInvoiceCommand = new RelayCommand(ExecuteCreateInvoice, CanExecuteCreateInvoice);
            PayInvoiceCommand = new RelayCommand(ExecutePayInvoice, CanExecutePayInvoice);

            LoadData();
        }

        public void LoadData()
        {
            // Chỉ lấy những phòng đang được thuê (Rented) để lập hóa đơn
            var rented = _roomService.GetAllRooms().Where(r => r.Status == RoomStatus.Rented);
            RentedRooms = new ObservableCollection<Room>(rented);

            // Load toàn bộ hóa đơn lên bảng
            Invoices = new ObservableCollection<Invoice>(_invoiceService.GetAllInvoices());
        }

        private bool CanExecuteCreateInvoice(object obj)
        {
            // Cần chọn phòng, tháng, năm hợp lệ và đơn giá điện nước phải > 0
            return SelectedRoom != null &&
                   SelectedMonth >= 1 && SelectedMonth <= 12 &&
                   SelectedYear > 2000 &&
                   ElectricPrice >= 0 &&
                   WaterPrice >= 0 &&
                   OtherFees >= 0;
        }

        private void ExecuteCreateInvoice(object obj)
        {
            bool isSuccess = _invoiceService.CreateInvoice(
                SelectedRoom.Id,
                SelectedMonth,
                SelectedYear,
                ElectricPrice,
                WaterPrice,
                OtherFees,
                out string error);

            if (isSuccess)
            {
                MessageBox.Show($"Tạo hóa đơn tháng {SelectedMonth}/{SelectedYear} thành công!", "Thông báo");
                LoadData(); // Tải lại bảng để thấy hóa đơn vừa tạo
                ClearForm();
            }
            else
            {
                MessageBox.Show(error, "Lỗi tạo hóa đơn", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecutePayInvoice(object obj)
        {
            return SelectedInvoice != null && !SelectedInvoice.IsPaid;
        }

        private void ExecutePayInvoice(object obj)
        {
            var confirm = MessageBox.Show(
                $"Xác nhận thu tiền hóa đơn tháng {SelectedInvoice.Month}/{SelectedInvoice.Year} của phòng này?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (confirm == MessageBoxResult.Yes)
            {
                bool isSuccess = _invoiceService.PayInvoice(SelectedInvoice.Id, out string error);

                if (isSuccess)
                {
                    MessageBox.Show("Thanh toán thành công!", "Thông báo");
                    LoadData(); // Cập nhật lại trạng thái IsPaid trên DataGrid
                }
                else
                {
                    MessageBox.Show(error, "Lỗi thanh toán", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearForm()
        {
            SelectedRoom = null;
            // Giữ nguyên tháng, năm, đơn giá điện nước để người dùng tiện lập cho phòng tiếp theo
            OtherFees = 0;
        }
    }
}