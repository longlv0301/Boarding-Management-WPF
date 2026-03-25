using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        // Lấy danh sách phòng trọ theo trạng thái
        IEnumerable<Room> GetRoomsByStatus(RoomStatus status);

        // Tìm chính xác 1 phòng thep tên
        Room GetRoomByName(string name);

        // Lấy thông tin kèm theo toàn bộ hợp đồng
        Room GetRoomWithDetails(int roomId);
    }
}
