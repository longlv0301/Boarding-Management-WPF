using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAllRooms();
        IEnumerable<Room> GetAvailableRooms();

        // Xem chi tiết
        Room GetRoomDetails(int roomId);

        bool AddRoom(Room room, out string errorMessage);
        bool UpdateRoom(Room room, out string errorMessage);

    }
}
