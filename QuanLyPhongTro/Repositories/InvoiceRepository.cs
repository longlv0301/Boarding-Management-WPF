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
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context) : base(context) { }
        
        public IEnumerable<Invoice> GetUnpaidInvoices()
        {
            return _dbSet.Where(i => i.IsPaid == false).ToList();
        }

        public IEnumerable<Invoice> GetInvoicesByPeriod(int month, int year)
        {
            return _dbSet.Where(i => i.Month ==  month && i.Year == year).ToList();
        }

        public IEnumerable<Invoice> GetInvoicesByRoom(int roomId)
        {
            return _dbSet
                .Where(i => i.RoomId == roomId)
                .OrderByDescending(i => i.Year)
                .ThenByDescending(i => i.Month)
                .ToList();
        }

        public Invoice GetInvoiceByRoomAndPeriod(int roomId, int month, int year)
        {
            return _dbSet.FirstOrDefault(i => i.RoomId == roomId && i.Month == month && i.Year == year);
        }

        public Invoice GetInvoiceWithDetails(int invoiceId)
        {
            return _dbSet
                .Include(i => i.Room)
                .FirstOrDefault(i => i.Id == invoiceId);
        }
    }
}
