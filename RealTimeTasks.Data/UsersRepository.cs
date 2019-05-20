using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealTimeTasks.Data
{
    public class UsersRepository
    {
        private string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;

            using (JobContext ctx = new JobContext(_connectionString))
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }

        public User GetByEmail(string email)
        {
            using (var ctx = new JobContext(_connectionString))
            {
                return ctx.Users
                    .FirstOrDefault(u => u.Email == email);
            }
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool correctPassword = PasswordHelper.PasswordMatch(password, user.PasswordHash);
            if (correctPassword)
            {
                return user;
            }

            return null;
        }
    }
}
