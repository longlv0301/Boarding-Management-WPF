using QuanLyPhongTro.Commands;
using QuanLyPhongTro.Models;
using QuanLyPhongTro.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            UpdateCommand = new RelayCommand(ExecuteUpdate, CanExecuteUpdate)
        }


    }
}
