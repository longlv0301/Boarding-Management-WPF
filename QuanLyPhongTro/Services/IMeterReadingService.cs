using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface IMeterReadingService
    {
        // Lấy lịch sử điện nước của 1 phòng
        IEnumerable<MeterReading> GetMeterReadingsByRoom(int roomId);

        // Lấy số liệu tháng gần nhất để tự động điền số cũ cho tháng mới
        MeterReading GetLatestMeterReading(int roomId);

        bool AddMeterReading(MeterReading meterReading, out string errorMessage);
        bool UpdateMeterReading(MeterReading meterReading, out string errorMessage);
    }
}
