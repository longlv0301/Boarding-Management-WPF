using QuanLyPhongTro.Commands;
using QuanLyPhongTro.Models;
using QuanLyPhongTro.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyPhongTro.ViewModels
{
    public class ContractViewModel : ViewModelBase
    {
        private readonly IContractService _contractService;
        private readonly IRoomService _roomService;
        private readonly ITenantService _tenantService;
        private readonly IOccupantService _occupantService;

        private List<Contract> _allContracts;

        private ObservableCollection<Contract> _contracts;
        public ObservableCollection<Contract> Contracts
        {
            get { return _contracts; }
            set { _contracts = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Room> _availableRooms;
        public ObservableCollection<Room> AvailableRooms
        {
            get { return _availableRooms; }
            set { _availableRooms = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Tenant> _tenants;
        public ObservableCollection<Tenant> Tenants
        {
            get { return _tenants; }
            set { _tenants = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Occupant> _currentOccupants;
        public ObservableCollection<Occupant> CurrentOccupants
        {
            get { return _currentOccupants; }
            set { _currentOccupants = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter(); 
            }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set { _selectedRoom = value; OnPropertyChanged(); }
        }

        private Tenant _selectedTenant;
        public Tenant SelectedTenant
        {
            get { return _selectedTenant; }
            set { _selectedTenant = value; OnPropertyChanged(); }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _endDate = DateTime.Now.AddMonths(6);
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
        }

        private decimal _depositAmount;
        public decimal DepositAmount
        {
            get { return _depositAmount; }
            set { _depositAmount = value; OnPropertyChanged(); }
        }

        private Contract _selectedContract;
        public Contract SelectedContract
        {
            get { return _selectedContract; }
            set
            {
                _selectedContract = value;
                OnPropertyChanged();

                if (_selectedContract != null)
                {
                    LoadOccupants(_selectedContract.Id);
                }
                else
                {
                    CurrentOccupants = new ObservableCollection<Occupant>();
                }
            }
        }

        private string _occFullName;
        public string OccFullName { get => _occFullName; set { _occFullName = value; OnPropertyChanged(); } }

        private string _occIdentityCard;
        public string OccIdentityCard { get => _occIdentityCard; set { _occIdentityCard = value; OnPropertyChanged(); } }

        private string _occPhoneNumber;
        public string OccPhoneNumber { get => _occPhoneNumber; set { _occPhoneNumber = value; OnPropertyChanged(); } }

        private string _occLicensePlate;
        public string OccLicensePlate { get => _occLicensePlate; set { _occLicensePlate = value; OnPropertyChanged(); } }

        private string _occAddress;
        public string OccAddress { get => _occAddress; set { _occAddress = value; OnPropertyChanged(); } }

        private Occupant _selectedOccupant;
        public Occupant SelectedOccupant { get => _selectedOccupant; set { _selectedOccupant = value; OnPropertyChanged(); } }

        public ICommand CreateContractCommand { get; }
        public ICommand TerminateContractCommand { get; }
        public ICommand ClearContractFormCommand { get; }
        public ICommand AddOccupantCommand { get; }
        public ICommand RemoveOccupantCommand { get; }

        public ContractViewModel(
            IContractService contractService,
            IRoomService roomService,
            ITenantService tenantService,
            IOccupantService occupantService)
        {
            _contractService = contractService;
            _roomService = roomService;
            _tenantService = tenantService;
            _occupantService = occupantService;

            CreateContractCommand = new RelayCommand(ExecuteCreateContract, CanExecuteCreateContract);
            TerminateContractCommand = new RelayCommand(ExecuteTerminateContract, CanExecuteTerminateContract);
            ClearContractFormCommand = new RelayCommand(ExecuteClearContractForm);

            AddOccupantCommand = new RelayCommand(ExecuteAddOccupant, CanExecuteAddOccupant);
            RemoveOccupantCommand = new RelayCommand(ExecuteRemoveOccupant, CanExecuteRemoveOccupant);

            LoadInitialData();
        }

        public void LoadInitialData()
        {
            var data = _contractService.GetAllContracts();
            if (data != null) _allContracts = data.ToList();
            else _allContracts = new List<Contract>();

            var rooms = _roomService.GetAvailableRooms();
            if (rooms != null) AvailableRooms = new ObservableCollection<Room>(rooms);

            var tenants = _tenantService.GetAllTenants();
            if (tenants != null) Tenants = new ObservableCollection<Tenant>(tenants);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allContracts == null) return;

            var filteredList = _allContracts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string searchLower = SearchText.ToLower();
                filteredList = filteredList.Where(c =>
                    c.Id.ToString().Contains(searchLower) ||
                    (c.Room != null && c.Room.Name != null && c.Room.Name.ToLower().Contains(searchLower)) ||
                    (c.Tenant != null && c.Tenant.FullName != null && c.Tenant.FullName.ToLower().Contains(searchLower))
                );
            }
            Contracts = new ObservableCollection<Contract>(filteredList);
        }

        public void LoadOccupants(int contractId)
        {
            var data = _occupantService.GetOccupantsByContract(contractId);
            if (data != null) CurrentOccupants = new ObservableCollection<Occupant>(data);
        }

        private bool CanExecuteCreateContract(object obj)
        {
            return SelectedRoom != null && SelectedTenant != null && DepositAmount >= 0 && EndDate > StartDate;
        }

        private void ExecuteCreateContract(object obj)
        {
            var newContract = new Contract
            {
                RoomId = SelectedRoom.Id,
                TenantId = SelectedTenant.Id,
                StartDate = StartDate,
                EndDate = EndDate,
                DepositAmout = DepositAmount
            };

            bool isSuccess = _contractService.CreateContract(newContract, out string error);

            if (isSuccess)
            {
                MessageBox.Show("Tạo hợp đồng thành công!", "Thông báo");
                LoadInitialData();
                ExecuteClearContractForm(null);
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteTerminateContract(object obj)
        {
            return SelectedContract != null && SelectedContract.IsActive;
        }

        private void ExecuteTerminateContract(object obj)
        {
            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn thanh lý hợp đồng này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                bool isSuccess = _contractService.TerminateContract(SelectedContract.Id, out string error);
                if (isSuccess)
                {
                    MessageBox.Show("Thanh lý thành công!", "Thông báo");
                    LoadInitialData();
                    SelectedContract = null;
                }
                else
                {
                    MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteClearContractForm(object obj)
        {
            SelectedRoom = null;
            SelectedTenant = null;
            DepositAmount = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(6);
        }

        private bool CanExecuteAddOccupant(object obj)
        {
            return SelectedContract != null && SelectedContract.IsActive &&
                   !string.IsNullOrWhiteSpace(OccFullName) &&
                   !string.IsNullOrWhiteSpace(OccIdentityCard) &&
                   !string.IsNullOrWhiteSpace(OccAddress); 
        }

        private void ExecuteAddOccupant(object obj)
        {
            var newOccupant = new Occupant
            {
                ContractId = SelectedContract.Id,
                FullName = OccFullName,
                IdentifyCard = OccIdentityCard,
                PhoneNumber = OccPhoneNumber,
                LicensePlate = string.IsNullOrWhiteSpace(OccLicensePlate) ? "" : OccLicensePlate,
                IsContractOwner = false
            };

            bool isSuccess = _occupantService.AddOccupant(newOccupant, out string error);

            if (isSuccess)
            {
                var existingTenant = Tenants.FirstOrDefault(t => t.IdentityCard == OccIdentityCard);

                if (existingTenant == null)
                {
                    var newTenant = new Tenant
                    {
                        FullName = OccFullName,
                        IdentityCard = OccIdentityCard,
                        PhoneNumber = OccPhoneNumber,
                        Address = OccAddress,
                        LicensePlate = string.IsNullOrWhiteSpace(OccLicensePlate) ? "" : OccLicensePlate, 
                        IsContractOwner = false 
                    };

                    _tenantService.AddTenant(newTenant, out string tenantError);

                    var tenants = _tenantService.GetAllTenants();
                    if (tenants != null)
                    {
                        Tenants = new ObservableCollection<Tenant>(tenants);
                    }
                }
    
                MessageBox.Show("Thêm người ở ghép thành công!", "Thông báo");

                LoadOccupants(SelectedContract.Id);

                OccFullName = string.Empty;
                OccIdentityCard = string.Empty;
                OccPhoneNumber = string.Empty;
                OccLicensePlate = string.Empty;
                OccAddress = string.Empty;
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteRemoveOccupant(object obj)
        {
            return SelectedOccupant != null;
        }

        private void ExecuteRemoveOccupant(object obj)
        {
            var confirm = MessageBox.Show($"Xóa nhân khẩu '{SelectedOccupant.FullName}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                bool isSuccess = _occupantService.RemoveOccupant(SelectedOccupant.Id, out string error);

                if (isSuccess)
                {
                    LoadOccupants(SelectedContract.Id);
                }
                else
                {
                    MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}