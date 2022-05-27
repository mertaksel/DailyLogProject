using Logger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DailyLogProject.Models;

namespace Logger.Controllers
{
    public class AccountController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            con.ConnectionString = DailyLogProject.Properties.Resources.ConnectionString;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }        
        [HttpPost]
        public IActionResult Login (AccountViewModel acc)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT * FROM UserLogin where Username=@Username and Passw0rd=@Passw0rd";
                com.Parameters.AddWithValue("@Username", acc.Username);
                com.Parameters.AddWithValue("@Passw0rd", acc.Passw0rd);




                var result = com.ExecuteReader();

                if (result != null && result.HasRows == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.HataMesaji = "Hatalı Kullanıcı Girişi!";
                    return View();
                }

                if (dr.Read())
                {
                    con.Close();
                    return View();
                }
                else
                {
                    con.Close();
                    return View();
                }
            }
            catch (Exception x)
            {

                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
