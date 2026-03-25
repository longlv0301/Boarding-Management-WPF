using QuanLyPhongTro.Data;
using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    internal class OccupantRepository : RepositoryBase<Occupant>, IOccupantRepository
    {
        public OccupantRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Occupant> GetOccupantsByContractId(int contractId)
        {
            return _dbSet.Where(o => o.ContractId == contractId).ToList();
        }

        public Occupant GetOccupantByIdentityCard(string identityCard)
        {
            return _dbSet.FirstOrDefault(o => o.IdentifyCard == identityCard); 
        }

        public Occupant GetContractOwner(int contractId)
        {
            return _dbSet.FirstOrDefault(o => o.ContractId == contractId && o.IsContractOwner == true);
        }
    }
}
