using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public decimal RoomFee { get; set; }
        public decimal ElectricFee { get; set; }
        public decimal WaterFee { get; set; }
        public decimal OtherFees { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? PaidDate { get; set; }
    }
}
