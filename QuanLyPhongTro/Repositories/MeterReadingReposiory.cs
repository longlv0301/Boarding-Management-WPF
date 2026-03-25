using QuanLyPhongTro.Data;
using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public class MeterReadingReposiory : RepositoryBase<MeterReading>, IMeterReadingRepositpry
    {
        public MeterReadingReposiory(AppDbContext context) : base(context) { }

        public MeterReading GetMeterReadingByPeriod(int roomId, int month, int year)
        {
            return _dbSet.FirstOrDefault(m => m.RoomId == roomId && m.Month == month && m.Year == year);
        }

        public MeterReading GetLatestMeterReading(int roomId)
        {
            return _dbSet
                .Where(m => m.RoomId == roomId)
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month)
                .FirstOrDefault();
        }

        public IEnumerable<MeterReading> GetMeterReadingsByRoom(int roomId)
        {
            return _dbSet
                .Where(m => m.RoomId == roomId)
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month)
                .ToList();
        }
    }
}
