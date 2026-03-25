using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface IContractService
    {
        IEnumerable<Contract> GetAllContracts();
        IEnumerable<Contract> GetActiveContracts();
        Contract GetContractDetails(int contractId);

        // Nghiệp vụ cốt lõi
        bool CreateContract(Contract contract, out string errorMessage);
        
        // Nghiệp vụ thanh lý/ kết thúc hợp đồng
        bool TerminalContract(int contractId, out string errorMessage);
    }
}
