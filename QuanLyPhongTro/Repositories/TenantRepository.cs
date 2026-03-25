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
    public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
    {
        public TenantRepository(AppDbContext context) : base(context) { } 

        public Tenant GetTenantByIdentityCard(string identityCard)
        {
            return _dbSet.FirstOrDefault(t => t.IdentityCard == identityCard);
        }

        public Tenant GetTenantByPhoneNumber(string phoneNumber)
        {
            return _dbSet.FirstOrDefault(p => p.PhoneNumber == phoneNumber);
        }

        public IEnumerable<Tenant> SearchTenantByName(string name)
        {
            if(string.IsNullOrEmpty(name)) return new List<Tenant>();
            return _dbSet.Where(t => t.FullName.ToLower().Contains(name.ToLower())).ToList();
        }
        public Tenant GetTenantWithContracts(int tenantId)
        {
            return _dbSet 
                .Include(t => t.Contracts)
                .FirstOrDefault(t => t.Id == tenantId);
        } 
    }
}
