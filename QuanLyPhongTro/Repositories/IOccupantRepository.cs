using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface IOccupantRepository : IRepository<Occupant>
    {
        // Lấy danh sách tất cả nhân khẩu đang ở chung 1 hợp đồng
        IEnumerable<Occupant> GetOccupantsByContractId(int contractId);

        // Tìm kiếm chính xác 1 người dựa vào số CCCD
        Occupant GetOccupantByIdentityCard(string identityCardId);

        // Lấy ra người đại diện đứng tên của 1 hợp đồng cụ thể
        Occupant GetContractOwner(int contractId);
    }
}
