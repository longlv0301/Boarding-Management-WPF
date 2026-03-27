using QuanLyPhongTro.Data;
using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        }

        public int GetTotalUsersCount()
        {
            return _dbSet.Count();
        }
    }
}
