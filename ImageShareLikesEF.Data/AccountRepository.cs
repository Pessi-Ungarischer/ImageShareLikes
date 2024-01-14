using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ImageShareLikes.Data;
using static System.Net.Mime.MediaTypeNames;

namespace AdsAuthentication.Data
{
    public class AccountRepository
    {

        private string _connectionString;
        public AccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Signup(User user)
        {
            using ImageDbContext context = new(_connectionString);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public bool Login(string email, string password)
        {
            using ImageDbContext context = new(_connectionString);
            User user = context.Users.FirstOrDefault(user => user.Email == email);

            if(user == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return false;

            }
            return true;
        }

    }
}
