using QuanLyPhongTro.Commands;
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

namespace QuanLyPhongTro.ViewModels
{
    public class TenantViewModel : ViewModelBase
    {
        private readonly ITenantService _tenantService;

        private List<Tenant> _allTenants;

        private ObservableCollection<Tenant> _tenants;
        public ObservableCollection<Tenant> Tenants
        {
            get { return _tenants; }
            set { _tenants = value; OnPropertyChanged(); }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _identityCard; // Số CCCD 
        public string IdentityCard
        {
            get { return _identityCard; }
            set { _identityCard = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        private string _licensePlate;
        public string LicensePlate { get => _licensePlate; set { _licensePlate = value; OnPropertyChanged(); } }

        private bool _isContractOwner = true; 
        public bool IsContractOwner { get => _isContractOwner; set { _isContractOwner = value; OnPropertyChanged(); } }

        private Tenant _selectedTenant;
        public Tenant SelectedTenant
        {
            get { return _selectedTenant; }
            set
            {
                _selectedTenant = value; OnPropertyChanged();
                if (_selectedTenant != null)
                {
                    FullName = _selectedTenant.FullName;
                    IdentityCard = _selectedTenant.IdentityCard;
                    PhoneNumber = _selectedTenant.PhoneNumber;
                    Address = _selectedTenant.Address;
                    LicensePlate = _selectedTenant.LicensePlate; 
                    IsContractOwner = _selectedTenant.IsContractOwner; 
                }
            }
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

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ClearFormCommand { get; }

        public TenantViewModel(ITenantService tenantService)
        {
            _tenantService = tenantService;

            AddCommand = new RelayCommand(ExecuteAdd, CanExecuteAdd);
            UpdateCommand = new RelayCommand(ExecuteUpdate, CanExecuteUpdate);
            ClearFormCommand = new RelayCommand(ExecuteClearForm);

            LoadTenants();
        }

        public void LoadTenants()
        {
            _allTenants = _tenantService.GetAllTenants().ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allTenants == null) return;

            var filteredList = _allTenants.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string searchLower = SearchText.ToLower();
                filteredList = filteredList.Where(t => (t.FullName != null && t.FullName.ToLower().Contains(searchLower)) ||
                                                       (t.PhoneNumber != null && t.PhoneNumber.Contains(searchLower))
                );
            }
            Tenants = new ObservableCollection<Tenant>(filteredList);
        }

        private bool CanExecuteAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrWhiteSpace(IdentityCard) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(Address);
        }

        private void ExecuteAdd(object obj)
        {
            var newTenant = new Tenant
            {
                FullName = FullName,
                IdentityCard = IdentityCard,
                PhoneNumber = PhoneNumber,
                Address = Address,
                LicensePlate = string.IsNullOrWhiteSpace(LicensePlate) ? "" : LicensePlate, 
                IsContractOwner = IsContractOwner 
            };

            bool isSuccess = _tenantService.AddTenant(newTenant, out string error);

            if (isSuccess)
            {
                MessageBox.Show("Thêm khách thuê thành công!", "Thông báo");
                LoadTenants();
                ExecuteClearForm(null);
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteUpdate(object obj)
        {
            return SelectedTenant != null &&
                   !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrWhiteSpace(IdentityCard) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(Address);
        }

        private void ExecuteUpdate(object obj)
        {
            SelectedTenant.FullName = FullName;
            SelectedTenant.IdentityCard = IdentityCard;
            SelectedTenant.PhoneNumber = PhoneNumber;
            SelectedTenant.Address = Address;
            SelectedTenant.LicensePlate = LicensePlate; 
            SelectedTenant.IsContractOwner = IsContractOwner; 

            bool isSuccess = _tenantService.UpdateTenant(SelectedTenant, out string error);

            if (isSuccess)
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                LoadTenants();
                ExecuteClearForm(null);
            }
            else
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteClearForm(object obj)
        {
            SelectedTenant = null;
            FullName = string.Empty;
            IdentityCard = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            LicensePlate = string.Empty;
            IsContractOwner = true; 
        }


    }
}
