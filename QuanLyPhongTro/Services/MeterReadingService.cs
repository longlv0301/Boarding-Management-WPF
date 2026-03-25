using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepositpry _meterReadingRepository;
        private readonly IRoomRepository _roomRepository; // Để kiểm tra phòng tồn tại không

        public MeterReadingService(IMeterReadingRepositpry meterReadingRepository, IRoomRepository roomRepository)
        {
            _meterReadingRepository = meterReadingRepository;
            _roomRepository = roomRepository;
        }

        public IEnumerable<MeterReading> GetMeterReadingsByRoom(int roomId)
        {
            return _meterReadingRepository.GetMeterReadingsByRoom(roomId);
        }

        public MeterReading GetLatestMeterReading(int roomId)
        {
            return _meterReadingRepository.GetLatestMeterReading(roomId);
        }

        public bool AddMeterReading(MeterReading meterReading, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Kiểm tra phòng có tồn tại không
            var room = _roomRepository.GetById(meterReading.RoomId);
            if(room == null)
            {
                errorMessage = "Phòng không tồn tại";
                return false;
            }

            // Logic số mới >= số cũ
            if(meterReading.ElectricNew < meterReading.ElectricOld)
            {
                errorMessage = "Số điện mới không được nhỏ hơn số điện cũ";
                return false;
            }

            if(meterReading.WaterNew < meterReading.WaterOld)
            {
                errorMessage = "Số nước mới không được nhỏ hơn số nước cũ";
                return false;
            }

            // Kiểm tra tháng/ năm này đã chốt số cho phòng này chưa
            var existingRecord = _meterReadingRepository.GetMeterReadingByPeriod(meterReading.RoomId, meterReading.Month, meterReading.Year);

            if(existingRecord != null)
            {
                errorMessage = $"Phòng {room.Name} đã được chốt điện nước cho tháng {meterReading.Month} rồi. Không thể tạo thêm!";
                return false;
            }
            _meterReadingRepository.Add(meterReading);
            _meterReadingRepository.SaveChanges();
            return true;
        }

        public bool UpdateMeterReading(MeterReading meterReading, out string errorMessage)
        {
            errorMessage= string.Empty;

            // Logic số mới >= số cũ
            if (meterReading.ElectricNew < meterReading.ElectricOld)
            {
                errorMessage = "Số điện mới không được nhỏ hơn số điện cũ";
                return false;
            }

            if (meterReading.WaterNew < meterReading.WaterOld)
            {
                errorMessage = "Số nước mới không được nhỏ hơn số nước cũ";
                return false;
            }

            // Kiểm tra trùng lặp
            var existingRecord = _meterReadingRepository.GetMeterReadingByPeriod(meterReading.RoomId, meterReading.Month, meterReading.Year);

            // Nếu tìm thấy 1 phiếu khác đang giữ tháng/năm này thì chặn lại
            if(existingRecord != null && existingRecord.Id != meterReading.Id)
            {
                errorMessage = $"Đã có phiếu ghi điện nước khác cho tháng {meterReading.Month}/{meterReading.Year}";
                return false;
            }
            _meterReadingRepository.Update(meterReading);
            _meterReadingRepository.SaveChanges();
            return true;
        }
    }
}
