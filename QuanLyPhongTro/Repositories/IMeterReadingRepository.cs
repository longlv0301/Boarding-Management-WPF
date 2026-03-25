using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface IMeterReadingRepository : IRepository<MeterReading>
    {
        // Kiểm tra xem phòng đã chốt điện nước trong tháng/ năm này chưa
        MeterReading GetMeterReadingByPeriod(int roomId, int month,  int year);

        // Lấy chỉ số điện nước mới nhất của 1 phòng
        MeterReading GetLatestMeterReading(int roomId);

        // Lấy lịch sử ghi điện nước của một phòng để hiển thị
        IEnumerable<MeterReading> GetMeterReadingsByRoom(int roomId);
    }
}
