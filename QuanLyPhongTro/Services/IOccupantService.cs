using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface IOccupantService
    {
        // Lấy danh sách người đang ở chung 1 phòng
        IEnumerable<Occupant> GetOccupantsByContract(int contractId);

        // Thêm người ở ghép
        bool AddOccupant(Occupant occupant, out string errorMessage);

        // Xóa/chuyển đi 1 người ở ghép
        bool RemoveOccupant(int occupantId, out string errorMessage);

    }
}
