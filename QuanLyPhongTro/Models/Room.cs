using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public int MaxOccupants { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        // Navigation properties
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<MeterReading> MeterReadings { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
    public enum RoomStatus { Available, Rented, Maintenance }
}
