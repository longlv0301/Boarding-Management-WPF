using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface IInvoiceService
    {
        // Truy xuất dữ liệu
        IEnumerable<Invoice> GetAllInvoices();
        IEnumerable<Invoice> GetUnpaidInvoices();
        IEnumerable<Invoice> GetInvoicesByRoom(int roomId);

        bool CreateInvoice(int roomId, int month, int year, decimal electricPrice, decimal waterPrice, decimal otherFees, out string errorMessage);
        bool DeleteInvoice(int invoiceId, out string errorMessage);
        // Xác nhận khách đã đóng tiền
        bool PayInvoice(int invoiceId, out string errorMessage);

    }
}
