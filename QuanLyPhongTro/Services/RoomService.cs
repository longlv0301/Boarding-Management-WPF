using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _roomRepository.GetAll();
        }

        public IEnumerable<Room> GetAvailableRooms()
        {
            return _roomRepository.GetRoomsByStatus(RoomStatus.Available);
        }

        public Room GetRoomDetails(int roomId)
        {
            return _roomRepository.GetRoomWithDetails(roomId);
        }

        public bool AddRoom(Room room, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!string.IsNullOrEmpty(room.Name))
            {
                room.Name = room.Name.Trim();
            }

            if(string.IsNullOrEmpty(room.Name))
            {
                errorMessage = "Tên phòng không được để trống";
                return false;
            }

            if(room.BasePrice <= 0)
            {
                errorMessage = "Giá phòng phải lớn hơn 0";
                return false;
            }

            if(room.MaxOccupants <= 0)
            {
                errorMessage = "Số người ở tối đa phải lớn hơn 0";
                return false;
            }

            var existingRoom = _roomRepository.GetRoomByName(room.Name);
            if(existingRoom != null)
            {
                errorMessage = $"Phòng mang tên '{room.Name}' đã tồn tại trong hệ thống";
                return false;
            }

            _roomRepository.Add(room);
            _roomRepository.SaveChanges();
            return true;
        }

        public bool UpdateRoom(Room room, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!string.IsNullOrEmpty(room.Name))
            {
                room.Name = room.Name.Trim();
            }

            if (string.IsNullOrEmpty(room.Name))
            {
                errorMessage = "Tên phòng không được để trống";
                return false;
            }

            if (room.BasePrice <= 0)
            {
                errorMessage = "Giá phòng phải lớn hơn 0";
                return false;
            }

            if (room.MaxOccupants <= 0)
            {
                errorMessage = "Số người ở tối đa phải lớn hơn 0";
                return false;
            }

            var existingByName = _roomRepository.GetRoomByName(room.Name);
            if(existingByName != null && existingByName.Id != room.Id)
            {
                errorMessage = $"Phòng mang tên '{room.Name}' đã tồn tại trong hệ thống";
                return false;
            }

            _roomRepository.Update(room);
            _roomRepository.SaveChanges();
            return true;
        }
    }
}
