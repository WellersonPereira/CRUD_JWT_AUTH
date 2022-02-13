using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_JWT_AUTH.Controllers
{
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
        public string Authenticated() => $"Auntenticated - {User.Identity.Name}";

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee, manager")]
        public string Employee() => "If I'm seeing this I'm a manager or a employee";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "I'm here because I'm a manager $_$";
    }
}