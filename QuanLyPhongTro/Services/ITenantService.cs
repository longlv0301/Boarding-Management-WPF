using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface ITenantService
    {
        // Truy xuất dữ liệu
        IEnumerable<Tenant> GetAllTenants();
        IEnumerable<Tenant> SearchTenants(string keyword);
        Tenant GetTenantDetails(int tenantId);

        // Thao tác dữ liệu với các quy tắc kiểm tra
        bool AddTenant(Tenant tenant, out string errorMesage);
        bool UpdateTenant(Tenant tenant, out string errorMesage);
    }
}
