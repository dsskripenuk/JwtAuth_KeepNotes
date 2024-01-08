using Drivers.Api.Configurations;
using Drivers.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Drivers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagmentController : ControllerBase
    {
        private readonly ILogger<AuthManagmentController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagmentController(
            ILogger<AuthManagmentController> logger, 
            UserManager<IdentityUser> userManager, 
            IOptionsMonitor<JwtConfig> optionsMonitor) 
        {
            _logger = logger;   
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto regRequest)
        {
            if (ModelState.IsValid)
            {
                var emailValid = await _userManager.FindByEmailAsync(regRequest.Email);

                if (emailValid != null)
                    return BadRequest("Email is already exist!");

                var newUser = new IdentityUser()
                {
                    Email = regRequest.Email,
                    UserName = regRequest.Email
                };

                var IsCreated = await _userManager.CreateAsync(newUser, regRequest.Password);

                if (IsCreated.Succeeded)
                {
                    return Ok(new RegistrationRequestResponce()
                    {
                        Result = true,
                        Token = GenerateJwtToken(newUser)
                    });
                }

                return BadRequest(IsCreated.Errors.Select(x => x.Description).ToList());
            }

            return BadRequest("Ivalid request");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto logRequest)
        {
            if(ModelState.IsValid)
            {
                var existUser = await _userManager.FindByEmailAsync(logRequest.Email);

                if (existUser == null)
                    return BadRequest("Invalid auth");

                var isPasswordValid = await _userManager.CheckPasswordAsync(existUser, logRequest.Password);

                if(isPasswordValid)
                {
                var token = GenerateJwtToken(existUser);

                    return Ok(new LoginRequestResponce()
                    {
                        Token = token,
                        Result = true
                    });
                }
            }

            return BadRequest("Ivalid request");
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
