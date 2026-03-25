using QuanLyPhongTro.Models;
using QuanLyPhongTro.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QuanLyPhongTro.Commands;

namespace QuanLyPhongTro.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;

        // Danh sách hiển thị trên bảng
        private ObservableCollection<Room> _rooms;
        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set { _rooms = value; OnPropertyChanged(); }
        }

        // Biến để hứng dữ liệu từ các ô TEXTBOX 
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; OnPropertyChanged(); }
        }

        private decimal _basePrice;
        public decimal BasePrice
        {
            get { return _basePrice; }
            set { _basePrice = value; OnPropertyChanged(); }
        }

        private int _maxOccupants;
        public int MaxOccupants
        {
            get { return _maxOccupants; }
            set { _maxOccupants = value; OnPropertyChanged(); }
        }

        // Biến lưu lại Phòng đang được click trên bảng
        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(); 
                
                if(_selectedRoom != null)
                {
                    RoomName = _selectedRoom.Name;
                    BasePrice = _selectedRoom.BasePrice;
                    MaxOccupants = _selectedRoom.MaxOccupants;
                }
            }
        }

        // Comands
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ClearFormCommand { get; }

        public RoomViewModel(IRoomService roomService)
        {
            _roomService = roomService;

            AddCommand = new RelayCommand(ExecuteAdd, CanExecuteAdd);
            UpdateCommand = new RelayCommand(ExecuteUpdate, CanExecuteUpdate);
            ClearFormCommand = new RelayCommand(ExecuteClearForm);

            LoadRooms();
        }

        public void LoadRooms()
        {
            var data = _roomService.GetAllRooms();
            Rooms = new ObservableCollection<Room>(data);
        }

        // Logic cho nút thêm
        private bool CanExecuteAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(RoomName) && BasePrice > 0 && MaxOccupants > 0;
        }
        private void ExecuteAdd(object obj)
        {
            var newRoom = new Room
            {
                Name = RoomName,
                BasePrice = BasePrice,
                MaxOccupants = MaxOccupants,
                Status = RoomStatus.Available
            };

            bool isSuccess = _roomService.AddRoom(newRoom, out string error);

            if(isSuccess)
            {
                MessageBox.Show("Thêm phòng thành công!", "Thông báo");
                LoadRooms();
                ExecuteClearForm(null);
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Logic cho nút sửa
        private bool CanExecuteUpdate(object obj)
        {
            return SelectedRoom != null && !string.IsNullOrWhiteSpace(RoomName) && BasePrice > 0;
        }

        private void ExecuteUpdate(object obj)
        {
            SelectedRoom.Name = RoomName;
            SelectedRoom.BasePrice = BasePrice;
            SelectedRoom.MaxOccupants = MaxOccupants;

            bool isSuccess = _roomService.UpdateRoom(SelectedRoom, out string error);

            if (isSuccess)
            {
                MessageBox.Show("Cập nhật thành công", "Thông báo");
                LoadRooms();
                ExecuteClearForm(null);
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Logic nút xóa trắng form
        private void ExecuteClearForm(object obj)
        {
            SelectedRoom = null;
            RoomName = string.Empty;
            BasePrice = 0;
            MaxOccupants = 0;
        }

    }
}
