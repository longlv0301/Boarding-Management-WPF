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
    public class MeterReadingViewModel : ViewModelBase
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IRoomService _roomService;

        private ObservableCollection<Room> _rentedRooms;
        public ObservableCollection<Room> RentedRooms { get => _rentedRooms; set { _rentedRooms = value; OnPropertyChanged(); } }

        private ObservableCollection<MeterReading> _roomHistory;
        public ObservableCollection<MeterReading> RoomHistory { get => _roomHistory; set { _roomHistory = value; OnPropertyChanged(); } }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();

                if (_selectedRoom != null)
                {
                    LoadRoomHistory();
                    AutoFillOldNumbers();
                }
                else
                {
                    RoomHistory = null;
                }
            }
        }

        private MeterReading _selectedRecord;
        public MeterReading SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
                if (_selectedRecord != null)
                {
                    Month = _selectedRecord.Month;
                    Year = _selectedRecord.Year;
                    ElectricOld = _selectedRecord.ElectricOld;
                    ElectricNew = _selectedRecord.ElectricNew;
                    WaterOld = _selectedRecord.WaterOld;
                    WaterNew = _selectedRecord.WaterNew;
                }
            }
        }

        private int _month = DateTime.Now.Month;
        public int Month { get => _month; set { _month = value; OnPropertyChanged(); } }

        private int _year = DateTime.Now.Year;
        public int Year { get => _year; set { _year = value; OnPropertyChanged(); } }

        private int _electricOld;
        public int ElectricOld { get => _electricOld; set { _electricOld = value; OnPropertyChanged(); } }

        private int _electricNew;
        public int ElectricNew { get => _electricNew; set { _electricNew = value; OnPropertyChanged(); } }

        private int _waterOld;
        public int WaterOld { get => _waterOld; set { _waterOld = value; OnPropertyChanged(); } }

        private int _waterNew;
        public int WaterNew { get => _waterNew; set { _waterNew = value; OnPropertyChanged(); } }

        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        public MeterReadingViewModel(IMeterReadingService meterReadingService, IRoomService roomService)
        {
            _meterReadingService = meterReadingService;
            _roomService = roomService;

            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            ClearCommand = new RelayCommand(ExecuteClear);

            LoadRentedRooms();
        }

        private void LoadRentedRooms()
        {
            var rooms = _roomService.GetAllRooms()?.Where(r => r.Status == RoomStatus.Rented).ToList();
            if (rooms != null) RentedRooms = new ObservableCollection<Room>(rooms);
        }

        private void LoadRoomHistory()
        {
            var history = _meterReadingService.GetMeterReadingsByRoom(SelectedRoom.Id);
            if (history != null) RoomHistory = new ObservableCollection<MeterReading>(history);
        }

        private void AutoFillOldNumbers()
        {
            var latest = _meterReadingService.GetLatestMeterReading(SelectedRoom.Id);
            if (latest != null)
            {
                ElectricOld = latest.ElectricNew;
                WaterOld = latest.WaterNew;
            }
            else
            {
                ElectricOld = 0;
                WaterOld = 0;
            }
            ElectricNew = ElectricOld;
            WaterNew = WaterOld;
        }

        private bool CanExecuteSave(object obj)
        {
            return SelectedRoom != null &&
                   Month >= 1 && Month <= 12 && Year > 2000 &&
                   ElectricNew >= ElectricOld &&
                   WaterNew >= WaterOld;
        }

        private void ExecuteSave(object obj)
        {
            var existingRecord = _meterReadingService.GetMeterReadingsByRoom(SelectedRoom.Id)
                                     .FirstOrDefault(m => m.Month == Month && m.Year == Year);

            bool isSuccess;
            string error;

            if (existingRecord != null)
            {
                var confirm = MessageBox.Show($"Tháng {Month}/{Year} đã có dữ liệu. Bạn có chắc chắn muốn CẬP NHẬT lại số liệu mới không?", "Xác nhận sửa đổi", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirm != MessageBoxResult.Yes) return;

                existingRecord.ElectricOld = ElectricOld;
                existingRecord.ElectricNew = ElectricNew;
                existingRecord.WaterOld = WaterOld;
                existingRecord.WaterNew = WaterNew;

                isSuccess = _meterReadingService.UpdateMeterReading(existingRecord, out error);
            }
            else
            {
                var newReading = new MeterReading
                {
                    RoomId = SelectedRoom.Id,
                    Month = Month,
                    Year = Year,
                    ElectricOld = ElectricOld,
                    ElectricNew = ElectricNew,
                    WaterOld = WaterOld,
                    WaterNew = WaterNew
                };

                isSuccess = _meterReadingService.AddMeterReading(newReading, out error);
            }

            if (isSuccess)
            {
                MessageBox.Show("Lưu số liệu thành công!", "Thông báo");
                LoadRoomHistory();
                SelectedRecord = null; 
                AutoFillOldNumbers(); 
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteClear(object obj)
        {
            SelectedRecord = null;
            if (SelectedRoom != null)
            {
                AutoFillOldNumbers(); // Trả lại số liệu tự động
                Month = DateTime.Now.Month;
                Year = DateTime.Now.Year;
            }
            else
            {
                ElectricOld = 0; ElectricNew = 0; WaterOld = 0; WaterNew = 0;
            }
        }
    }
}