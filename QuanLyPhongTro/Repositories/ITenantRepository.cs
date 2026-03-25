using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        // Kiểm tra khách này đã thuê trọ ở hệ thống chưa bằng CCCD
        Tenant GetTenantByIdentityCard(string identityCard);

        // Tìm khách bằng số điện thoại 
        Tenant GetTenantByPhoneNumber(string phoneNumber);

        // Tìm kiến gần đúng theo tên
        IEnumerable<Tenant> SearchTenantByName(string name);

        // Lấy thông tin khách hàng kèm theo lịch sử các hợp đồng
        Tenant GetTenantWithContracts(int tenantId);
    }
}
