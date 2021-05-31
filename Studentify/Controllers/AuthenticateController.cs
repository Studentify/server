using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Studentify.Data.Repositories;
using Studentify.Models.Authentication;
using Studentify.Models.HttpBody;

namespace Studentify.Controllers
{
    /// <summary>
    /// Controller responsible for logging and registering user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<StudentifyUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IStudentifyAccountsRepository _accountsRepository;

        public AuthenticateController(UserManager<StudentifyUser> userManager, IConfiguration configuration, IStudentifyAccountsRepository accountsRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _accountsRepository = accountsRepository;
        }

        /// <summary>
        /// Logging user with Studentify.Models.Authentication.LoginModel
        /// </summary>
        /// <param name="dto">
        /// User's credentials eg.
        /// {
        ///     "Username": "user",
        ///     "Password": "password"
        /// }
        /// </param>
        /// <returns>
        /// In case of success returns code 200 with JWT token.
        /// Otherwise returns http code 401 Unauthorized.
        /// </returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password)) 
                return Unauthorized();
            
            var token = await GenerateToken(user);
            var account = await _accountsRepository.SelectByUsername(dto.Username);
            return Ok(new
            {
                
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                account
            });
        }
        
        /// <summary>
        /// Register user in database with Studentify.Models.Authentication.RegisterModel
        /// and creates StudentifyAccount for him.
        /// </summary>
        /// <param name="dto">
        /// New user's credentials eg.
        /// {
        ///     "Username": "user",
        ///     "Email": "email@test.com"
        ///     "Password": "password"
        /// }
        /// Password have to contain one uppercase letter,
        /// one digit and one non alphanumeric character
        /// </param>
        /// <returns>
        /// In case of success returns code 200.
        /// Otherwise returns code 500 with an error description.
        /// </returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new StudentifyUser()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Username
            };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors;
                return StatusCode(StatusCodes.Status500InternalServerError, errors);
            }

            await _accountsRepository.InsertFromStudentifyUser(user);

            var token = await GenerateToken(user);
            
            return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
        }
        
        private async Task<JwtSecurityToken> GenerateToken(StudentifyUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
