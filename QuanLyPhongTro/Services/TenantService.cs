using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public IEnumerable<Tenant> GetAllTenants()
        {
            return _tenantRepository.GetAll();
        }

        public IEnumerable<Tenant> SearchTenants(string keyword)
        {
            // Nếu không nhập gì ở ô tìm kiếm, trả về toàn bộ danh sách
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return _tenantRepository.GetAll();
            }

            return _tenantRepository.SearchTenantByName(keyword);
        }

        public Tenant GetTenantDetails(int tenantId)
        {
            return _tenantRepository.GetTenantWithContracts(tenantId);
        }

        public bool AddTenant(Tenant tenant, out string errorMesage)
        {
            errorMesage = string.Empty;
            
            // Kiểm tra dữ liệu bắt buộc
            if(string.IsNullOrWhiteSpace(tenant.FullName) ||
               string.IsNullOrWhiteSpace(tenant.IdentityCard) ||
               string.IsNullOrWhiteSpace(tenant.PhoneNumber))
            {
                errorMesage = "Họ tên, CCCD và Số điện thoại không được để trống";
                return false;
            }

            // Kiểm tra độ dài CCCD
            if(tenant.IdentityCard.Length != 12)
            {
                errorMesage = "Số CCCD phải bao gồm đúng 12 chữ số";
                return false;
            }

            // Kiểm tra trùng lặp CCCD
            var existingByIdentity = _tenantRepository.GetTenantByIdentityCard(tenant.IdentityCard);
            if(existingByIdentity != null)
            {
                errorMesage = $"Khách hàng có số CCCD {tenant.IdentityCard} đã tồn tại trong hệ thống!";
                return false;
            }
            
            // Kiểm tra độ dài Số điện thoại
            if(tenant.PhoneNumber.Length != 10)
            {
                errorMesage = "Số điện thoại phải bao gồm đúng 10 chữ số";
                return false;
            }

            // Kiểm tra trùng lặp Số điện thoại
            var existingByPhone = _tenantRepository.GetTenantByPhoneNumber(tenant.PhoneNumber);
            if(existingByPhone != null)
            {
                errorMesage = $"Khách hàng có Số điện thoại {tenant.PhoneNumber} đã tồn tại trong hệ thống!";
                return false;
            }

            _tenantRepository.Add(tenant);
            _tenantRepository.SaveChanges();
            return true;
        }

        public bool UpdateTenant(Tenant tenant, out string errorMesage)
        {
            errorMesage = string.Empty;

            // Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(tenant.FullName) ||
               string.IsNullOrWhiteSpace(tenant.IdentityCard) ||
               string.IsNullOrWhiteSpace(tenant.PhoneNumber))
            {
                errorMesage = "Họ tên, CCCD và Số điện thoại không được để trống";
                return false;
            }

            // Kiểm tra độ dài CCCD
            if (tenant.IdentityCard.Length != 12)
            {
                errorMesage = "Số CCCD phải bao gồm đúng 12 chữ số";
                return false;
            }

            // Kiểm tra trùng lặp CCCD
            var existingByIdentity = _tenantRepository.GetTenantByIdentityCard(tenant.IdentityCard);
            if(existingByIdentity != null && existingByIdentity.Id != tenant.Id)
            {
                errorMesage = $"Khách hàng có số CCCD {tenant.IdentityCard} đã tồn tại trong hệ thống!";
                return false;
            }

            // Kiểm tra độ dài Số điện thoại
            if (tenant.PhoneNumber.Length != 10)
            {
                errorMesage = "Số điện thoại phải bao gồm đúng 10 chữ số";
                return false;
            }

            // Kiểm tra trùng lặp Số điện thoại
            var existingByPhone = _tenantRepository.GetTenantByPhoneNumber(tenant.PhoneNumber);
            if(existingByPhone != null && existingByPhone.Id != tenant.Id)
            {
                errorMesage = $"Khách hàng có Số điện thoại {tenant.PhoneNumber} đã tồn tại trong hệ thống!";
                return false;
            }

            _tenantRepository.Update(tenant);
            _tenantRepository.SaveChanges();
            return true;
        }
    }
}
