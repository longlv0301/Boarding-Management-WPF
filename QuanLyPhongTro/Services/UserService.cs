using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool IsFirstRun()
        {
            return _userRepository.GetTotalUsersCount() == 0;
        }

        public bool CreateInitialAdmin(string username, string password, string fullName, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!IsFirstRun())
            {
                errorMessage = "Hệ thống đã được khởi tạo từ trước. Không thể tạo tài khoản theo cách này";
                return false;
            }

            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Tài khoản và Mật khẩu không được để trống";
                return false;
            }

            var adminUser = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                FullName = fullName,
                Role = UserRole.Admin
            };

            _userRepository.Add(adminUser);
            _userRepository.SaveChanges();
            return true;
        }

        public User Authenticate(string username, string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Tài khoản và Mật khẩu không được để trống";
                return null;
            }

            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                errorMessage = "Tài khoản không tồn tại";
                return null;
            }

            string inputHash = HashPassword(password);
            if(user.PasswordHash != inputHash)
            {
                errorMessage = "Mật khẩu không chính xác";
                return null;
            }

            return user;
        }
    }
}
