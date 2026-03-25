using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        // Lấy danh sách các hóa đơn chưa thanh toán
        IEnumerable<Invoice> GetUnpaidInvoices();

        // Lấy tất cả hóa đơn của 1 tháng/ năm cụ thể (doanh thu)
        IEnumerable<Invoice> GetInvoicesByPeriod(int month, int year);

        // Lấy lịch sử tất cả hóa đơn của 1 phòng cụ thể
        IEnumerable<Invoice> GetInvoicesByRoom(int roomId);

        // Kiểm tra xem phòng này trong tháng này đã tạo hóa đơn chưa
        Invoice GetInvoiceByRoomAndPeriod(int roomId, int month, int year);

        // Lấy chi tiết một hóa đơn kèm theo thông tin của phòng
        Invoice GetInvoiceWithDetails(int invoiceId);
    }
}
