using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_JWT_AUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string anonymous() => "I don't need to authenticate to get here =)";

        [HttpGet]
        [Route("authenticated")]
        [Authorize()]
        public string Authenticated() => $"Authenticated - {User.Identity.Name}";

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "Admin, User")]
        public string Employee() => "If I'm seeing this I'm a Admin or a User";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "Admin")]
        public string Manager() => "I'm here because I'm a Admin $_$";
    }
}