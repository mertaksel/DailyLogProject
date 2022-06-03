using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using Logger.Models;
using DailyLogProject.Entities;
using DailyLogProject.Helpers;
using DailyLogProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DailyLogProject.Services
{
    public interface IUserService
    {
        User Authenticate(AccountViewModel acc);
        IEnumerable<User> GetAllByAccountInfo(AccountViewModel acc);
        IEnumerable<User> Insert(User user);
        bool IsUserExist(User user);
    }

    public class UserService : IUserService
    {

        public IUserService _userService;
        public IConfiguration _configuration;
        private readonly AppSettings appSettings;

        public UserService(IConfiguration configuration,IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            _configuration = configuration;
        }


        public User Authenticate(AccountViewModel acc)
        {
            var users = GetAllByAccountInfo(acc);
            
            User authenticatedUser=null;

            foreach (var user in users)
            {
                if (user.Passw0rd == acc.Passw0rd && user.Username == acc.Username)
                {
                    authenticatedUser = user;
                    break;
                }
            }

            if(authenticatedUser == null)
                throw new Exception("Hatalı Kullanıcı Girişi!");

            
            return authenticatedUser;
        }

        public IEnumerable<User> GetAllByAccountInfo(AccountViewModel acc)
        {
            var conStr = _configuration.GetConnectionString("DefaultConnection");
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(conStr);

            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT top 1 * FROM UserLogin where Username=@Username and Passw0rd=@Passw0rd";
            com.Parameters.AddWithValue("@Username", acc.Username);
            com.Parameters.AddWithValue("@Passw0rd", acc.Passw0rd);
            var result = com.ExecuteReader();
            


            if (result == null || !result.HasRows == true)
                throw new Exception("Hatalı Kullanıcı Girişi!");

            List<User> users = new List<User>();

            while (result.Read())
            {
                users.Add(new User()
                {
                    Username = result["Username"].ToString(),
                    Passw0rd = result["Passw0rd"].ToString()
                });
            }

            con.Close();

            return users;
        }

        public IEnumerable<User> Insert(User user)
        {
            throw new NotImplementedException();
        }

        public bool IsUserExist(User user)
        {
            throw new NotImplementedException();
        }
    }
}