using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    internal class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, IRoomRepository roomRepository, IMeterReadingRepository meterReadingRepository)
        {
            _invoiceRepository = invoiceRepository;
            _roomRepository = roomRepository;
            _meterReadingRepository = meterReadingRepository;
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _invoiceRepository.GetAll();
        }

        public IEnumerable<Invoice> GetUnpaidInvoices()
        {
            return _invoiceRepository.GetUnpaidInvoices();
        }

        public IEnumerable<Invoice> GetInvoicesByRoom(int roomId)
        {
            return _invoiceRepository.GetInvoicesByRoom(roomId);
        }

        public bool CreateInvoice(int roomId, int month, int year, decimal electricPrice, decimal waterPrice, decimal otherFees, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Kiểm tra chống tạo trùng hóa đơn
            var existingInvoice = _invoiceRepository.GetInvoiceByRoomAndPeriod(roomId, month, year);
            if(existingInvoice != null)
            {
                errorMessage = $"Phòng này đã xuất hóa đơn cho tháng {month}/{year} rồi!";
                return false;
            }
            
            // Kéo thông tin phòng lên để lấy giá thuê gốc
            var room = _roomRepository.GetById(roomId);
            if(room == null)
            {
                errorMessage = "Không tìm thấy phòng!";
                return false;
            }

            // Kéo thông tin điện nước của tháng lên
            var meter = _meterReadingRepository.GetMeterReadingByPeriod(roomId, month, year);
            if(meter == null)
            {
                errorMessage = $"Phòng này chưa được chốt điện nước tháng {month}/{year}. Vui lòng chốt số liệu trước khi tính tiền!";
                return false;
            }
            
            // TÍNH TOÁN
            decimal electricFee = (meter.ElectricNew - meter.ElectricOld) * electricPrice;
            decimal waterFee = (meter.WaterNew - meter.WaterOld) * waterPrice;

            // TẠO HÓA ĐƠN
            var newInvoice = new Invoice
            {
                RoomId = roomId,
                Month = month,
                Year = year,
                RoomFee = room.BasePrice,
                ElectricFee = electricFee,
                WaterFee = waterFee,
                OtherFees = otherFees,
                IsPaid = false,
                PaidDate = null
            };
            
            _invoiceRepository.Add(newInvoice);
            _invoiceRepository.SaveChanges();
            return true;
        }

        public bool PayInvoice(int invoiceId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var invoice = _invoiceRepository.GetById(invoiceId);
            if(invoice == null)
            {
                errorMessage = "Không tìm thấy hóa đơn!";
                return false;
            }

            // Kiểm tra hóa đơn này đã được thu tiền trước chưa
            if (invoice.IsPaid)
            {
                errorMessage = "Hóa đơn này đã được thu tiền từ trước rồi!";
                return false;
            }

            invoice.IsPaid = true;
            invoice.PaidDate = DateTime.Now;

            _invoiceRepository.Update(invoice);
            _invoiceRepository.SaveChanges();
            return true;
        }
    }
}
