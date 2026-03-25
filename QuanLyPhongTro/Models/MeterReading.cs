using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Models
{
    public class MeterReading
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public int ElectricOld { get; set; }
        public int ElectricNew { get; set; }
        public int WaterOld { get; set; }
        public int WaterNew { get; set; }
    }
}
