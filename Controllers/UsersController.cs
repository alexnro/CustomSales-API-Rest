using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using APIRestCustomSales.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRestCustomSales.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly UsersService _usersService;

        public UsersController(UsersService usersService) {
            _usersService = usersService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() {
            return _usersService.GetUsers();
        }

        [HttpGet("login")]
        public ActionResult<User> GetByName([FromBody] LoginUser loginUser) {
            return _usersService.HandleLogin(loginUser);
        }

    }

}