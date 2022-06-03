using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyLogProject.Entities;
using DailyLogProject.Services;
using Logger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyLogProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]AccountViewModel userParam)
        {

            var user = _userService.Authenticate(userParam);

            if (user == null)
                return BadRequest(new { message = "Kullanici veya şifre hatalı!" });

            return Ok(user);
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var users = _userService.GetAllByAccountInfo();
        //    return Ok(users);
        //}


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            bool isUserExist;
            isUserExist = _userService.IsUserExist(user);

            if (isUserExist)
                return BadRequest("Kullanıcı adı sistemde kayıtlıdır.");

            var users = _userService.Insert(user);
            return Ok(users);
        }
    }
}