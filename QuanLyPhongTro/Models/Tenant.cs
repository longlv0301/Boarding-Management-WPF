using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string IdentityCard { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string LicensePlate { get; set; }
        public bool IsContractOwner { get; set; } = true;
        public ICollection<Contract> Contracts { get; set; }
    }
}
