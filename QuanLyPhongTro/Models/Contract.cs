using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Models
{
    public class Contract
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DepositAmout { get; set; }
        public bool IsActive { get; set; } = true;

        // Quản lý danh sách nhân khẩu thực tế ăn ngủ tại phòng này
        public ICollection<Occupant> Occupants { get; set; }
    }
}
