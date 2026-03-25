using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        int GetTotalUsersCount();
    }
}
