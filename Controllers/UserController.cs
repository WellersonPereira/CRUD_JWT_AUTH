using CRUD_JWT_AUTH.Dtos;
using CRUD_JWT_AUTH.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace CRUD_JWT_AUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserController(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              RoleManager<IdentityRole<int>> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            return Ok(new UserDto());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                foreach (var role in userDto.Roles)
                {
                    if(!(await _roleManager.RoleExistsAsync(role)))
                        await _roleManager.CreateAsync(new IdentityRole<int>(role));
                }

                // if(!(await _roleManager.RoleExistsAsync(userDto.Roles)))
                //         await _roleManager.CreateAsync(new IdentityRole<int>(userDto.Roles));

                var user = new User
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    FullName = userDto.FullName
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);
                var resultRole = await _userManager.AddToRolesAsync(user, userDto.Roles);

                if (result.Succeeded)
                    return Created("GetUser", user);
                else
                    return BadRequest(result.Errors);

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                if (result.Succeeded)
                {
                    var appUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());

                    var token = GenerateJWT(appUser);

                    return Ok(new
                    {
                        token = GenerateJWT(appUser).Result
                    });
                }
                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
            }
        }

        private async Task<string> GenerateJWT(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Settings.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = credential
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}