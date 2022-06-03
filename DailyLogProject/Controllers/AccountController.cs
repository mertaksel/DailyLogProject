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
using Microsoft.AspNetCore.Session;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DailyLogProject.Services;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using DailyLogProject.Helpers;
using Microsoft.Extensions.Options;

namespace Logger.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;
        private IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger, IOptions<AppSettings> appSettings, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _appSettings = appSettings.Value;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AccountViewModel acc)
        {

            var authenticatedUser = _userService.Authenticate(acc);

            if (authenticatedUser == null)
                return RedirectToAction("Login");

            var token = Extensions.CreateJWT(acc.Username, acc.Passw0rd);
            HttpContext.Request.Headers.Add("Authorization", $"Bearer {token}");

            //return Ok(new { token = token });

            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
