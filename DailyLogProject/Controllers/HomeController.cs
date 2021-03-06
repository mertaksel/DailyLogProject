using DailyLogProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DailyLogProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //SQL veritabanına gittiğimizde dataları kontrol etmek için ve benzeri işler için vericeğimiz sql kodlarını gidip dbye verileri alıyor geri bize getiriyor.
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();

        private readonly ILogger<HomeController> _logger;
        //Koda Hangi SQL tablosuna bağlamasını göstermek için Resources tablosuna yönlendiriyoruz SQL Database bilgileri Resources'ta
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = DailyLogProject.Properties.Resources.ConnectionString;
        }
       
        //Modele gidip Title ve Descrip datasına gidip getiriyor.
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();

            viewModel.AddressList = FetchAddresData();

            return View(viewModel);
        }
        //HTTPPost Database'e kayıt eklememize yarıyor.
        [HttpPost]
        public IActionResult Index(IndexViewModel model)
        {
            try
            {
                //Bağlantımı Açıyorum SQL Tablosuna
                con.Open();
                com.Connection = con;
                //Aşağıdaki kodla ' gibi karakterlerde hata almamıza yarıyor.
                var prm1 = new SqlParameter("", System.Data.SqlDbType.NVarChar);
                prm1.Value = "";
                com.Parameters.Add(prm1);
                //Kodumuz SQL Tablosuna kayıt eklemesi için yapması gereken kodu yazıyoruz "" içerisinde ki kod
                com.CommandText = $"Insert into [KumportMert].[dbo].[DailyLog](Title,Descrip) values ('{model.AddressData.Title.Replace("'","''")}','{model.AddressData.Descrip.Replace("'","''")}')";
                var x = com.ExecuteNonQuery();

                con.Close();
            }
            // Kodda  hata durumunda bize hata fırlatmaya yarıyor.
            catch (Exception )
            {
                throw;
            }
            // Kayıt İşlemimiz tamamlandığında sayfamızda DBdeki değerler gösterilmesine yarıyor sayfayı refresh etmemize gerek kalmıyor.
            return View("Index", "Home");
        }
        private List<AddressViewModel> FetchAddresData()
        {
            var addressList = new List<AddressViewModel>();
            //Burası [HTTPGET] aslında Databaseden verileri çağırıyoruz(Database gidip verileri alıp geri getiriyoruz).
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [MakeDate], [Title],[Descrip] FROM [KumportMert].[dbo].[DailyLog] ORDER BY MakeDate DESC";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    addressList.Add(new AddressViewModel()

                    {
                        MakeDate = dr["MakeDate"].ToString()
                        ,Title = dr["Title"].ToString()                 
                        ,Descrip = dr["Descrip"].ToString()
                    });
                }
                con.Close();
            }
            // Hata Fırlatıyoruz.
            catch (Exception)
            {
                throw ;
            }

            return addressList;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
