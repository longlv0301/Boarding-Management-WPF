using QuanLyPhongTro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public interface IUserService
    {
        bool IsFirstRun();
        bool CreateInitialAdmin(string username, string password, string fullName, out string errorMessage);
        User Authenticate(string username, string password, out string errorMessage);
    }
}
