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

        private ObservableCollection<Tenant> _tenants;
        public ObservableCollection<Tenant> Tenants
        {
            get { return _tenants; }
            set { _tenants = value; OnPropertyChanged(); }
        }

        private string _fullName;
        private string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _identityCard; // Số CCCD / CMND
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

        private Tenant _selectedTenant;
        public Tenant SelectedTenant
        {
            get { return _selectedTenant; }
            set
            {
                _selectedTenant = value; OnPropertyChanged();
                if(_selectedTenant != null)
                {
                    FullName = _selectedTenant.FullName;
                    IdentityCard = _selectedTenant.IdentityCard;
                    PhoneNumber = _selectedTenant.PhoneNumber;
                    Address = _selectedTenant.Address;
                }
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
            var data = _tenantService.GetAllTenants();
            Tenants = new ObservableCollection<Tenant>(data);
        }

        private bool CanExecuteAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrWhiteSpace(IdentityCard) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber);
        }

        private void ExecuteAdd(object obj)
        {
            var newTenant = new Tenant
            {
                FullName = FullName,
                IdentityCard = IdentityCard,
                PhoneNumber = PhoneNumber,
                Address = Address
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
                   !string.IsNullOrWhiteSpace(PhoneNumber);
        }

        private void ExecuteUpdate(object obj)
        {
            SelectedTenant.FullName = FullName;
            SelectedTenant.IdentityCard = IdentityCard;
            SelectedTenant.PhoneNumber = PhoneNumber;
            SelectedTenant.Address = Address;

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
        }


    }
}
