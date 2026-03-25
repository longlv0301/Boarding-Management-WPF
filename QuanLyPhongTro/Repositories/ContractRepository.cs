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
    public class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext context) : base(context) { }
        public IEnumerable<Contract> GetActiveContracts()
        {
            return _dbSet.Where(c => c.IsActive == true && c.EndDate >= DateTime.Now).ToList();
        }

        public Contract GetContractWithDetails(int id)
        {
            return _dbSet
                .Include(c => c.Room)
                .Include(c => c.TenantId)
                .Include(c => c.Occupants)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Contract> GetContractsByRoom(int roomId)
        {
            return _dbSet.Where(c => c.RoomId == roomId).ToList();
        }

        public IEnumerable<Contract> GetContractByTenant(int tenantId)
        {
            return _dbSet.Where(c => c.TenantId == tenantId).ToList();
        }

        public Contract GetActiveContractByRoom(int roomId)
        {
            return _dbSet.FirstOrDefault(c => c.RoomId == roomId && c.IsActive == true);
        }
    }
}
