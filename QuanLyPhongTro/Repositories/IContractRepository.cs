using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface IContractRepository : IRepository<Contract>
    {
        // Lấy danh sách các hợp đồng còn hiệu lực
        IEnumerable<Contract> GetActiveContracts();

        // Lấy chi tiết 1 hợp đồng kèm them thông tin phòng, người đại diện và danh sách nhân khẩu
        Contract GetContractWithDetails(int id);

        // Lấy lịch sử tất cả hợp động của một phòng cụ thể
        IEnumerable<Contract> GetContractsByRoom(int roomId);

        // Lấy lịch sử tất cả hợp đồng của người thuê cụ thể
        IEnumerable<Contract> GetContractByTenant(int tenantId);

        // Lấy hợp đồng đang có hiệu lực của 1 phòng
        Contract GetActiveContractByRoom(int roomId);

    }
}
