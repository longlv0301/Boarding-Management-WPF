using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Models
{
    public class Occupant
    {
        public int Id { get; set; }

        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        public string FullName { get; set; }
        public string IdentifyCard { get; set; }
        public string PhoneNumber { get; set; }
        public string LicensePlate { get; set; }
        public bool IsContractOwner { get; set; } = false;
    }
}
