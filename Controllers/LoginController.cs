using CRUD_JWT_AUTH.Models;
using CRUD_JWT_AUTH.Repositories;
using CRUD_JWT_AUTH.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_JWT_AUTH.Controllers
{
    [ApiController]
    [Route("v1")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            //Get the user
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest("Username or password invalid");
            }

            var token = TokenService.GenerateToken(user);
            user.Password = "";

            return new {user = user,
            token = token};
        }
    }
}