using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using APIRestCustomSales.Models;
using APIRestCustomSales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRestCustomSales.Controllers {

    [Authorize]
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

        [HttpPost]
        public ActionResult<User> Create(User newUser) {
            if (_usersService.GetUserByUsername(newUser.Username) != null) {
                return NoContent();
            }

            return _usersService.AddUser(newUser);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginUser loginUser) {
            return _usersService.HandleLogin(loginUser);
        }

        [AllowAnonymous]
        [HttpPost("createPassword")]
        public ActionResult<User> CreatePassword(LoginUser loginUser) {
            return _usersService.HandleCreatePassword(loginUser);
        }

        [HttpPost("authenticate")]
        public ActionResult<User> GetAuthUser([FromBody] TokenAuth tokenAuth) {
            User user = _usersService.GetUserByToken(tokenAuth.Token);

            if (user.Password == null) return null;

            return user;
        }

        [HttpPut]
        public IActionResult Update(User updatedUser) {
            if (_usersService.GetUserById(updatedUser.Id) == null) {
                return NotFound();
            }

            _usersService.Update(updatedUser);
            return NoContent();
        }

        [HttpPut("logout")]
        public IActionResult Logout([FromBody] LogoutUser logoutUser) {
            var user = _usersService.GetUserByUsername(logoutUser.Username);

            if (user == null) {
                return NotFound();
            }

            _usersService.HandleLogout(user);
            return NoContent();
        }

        [HttpPut("resetPassword")]
        public IActionResult ResetPassword(User userToReset) {
            var user = _usersService.GetUserById(userToReset.Id);
            if (user.Id == null) {
                return NotFound();
            }

            _usersService.ResetPassword(user);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(string userId) {
            var user = _usersService.GetUserById(userId);

            if (user == null) {
                return NotFound();
            }

            _usersService.Delete(userId);
            return NoContent();
        }

    }

}