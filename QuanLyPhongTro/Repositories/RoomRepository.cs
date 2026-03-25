using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.Data;
using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    // Kế thừa IRoomRepository để đảm bảo thực thi các hàm đặc thù
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        // Truyền context cho class cha xứ lý
        public RoomRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Room> GetRoomsByStatus(RoomStatus status)
        {
            return _dbSet.Where(r => r.Status == status).ToList();
        }

        public Room GetRoomByName(string name)
        {
            return _dbSet.FirstOrDefault(r => r.Name == name);
        }

        public Room GetRoomWithDetails(int roomId)
        {
            return _dbSet
                .Include(r => r.Contracts)
                .Include(r => r.MeterReadings)
                .Include(r => r.Invoices)
                .FirstOrDefault(r => r.Id == roomId);
        }
    }
}
