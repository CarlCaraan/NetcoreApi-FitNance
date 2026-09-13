using FitNance.Data;
using FitNance.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; //JWT
using System.IdentityModel.Tokens.Jwt; //JWT
using System.Security.Claims;  //JWT
using System.Text; //JWT

using Microsoft.AspNetCore.Authorization;


namespace FitNance.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController( AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers() // FOR TESTING
        {
            var users = await _context.UserLogin.ToListAsync();

            return Ok(users);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingUser = await _context.UserLogin
                .FirstOrDefaultAsync(x => x.Username == request.Username);

            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var user = new UserLoginModel
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,

                Username = request.Username,
                Password = request.Password,
                CreatedBy = request.Username,
                CreatedDate = DateTime.Now,
                IsProfileComplete = false
            };

            var passwordHasher = new PasswordHasher<UserLoginModel>();

            user.Password = passwordHasher.HashPassword(
                user,
                request.Password
            );

            _context.UserLogin.Add(user);

            await _context.SaveChangesAsync();

            // ==========================================
            // CREATE JWT
            // ==========================================

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username!),
        new Claim("Rowstamp", user.Rowstamp.ToString()),
        new Claim("IsProfileComplete", user.IsProfileComplete.ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        _configuration["Jwt:ExpiresInMinutes"]
                    )
                ),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler()
                .WriteToken(token);


            // ==========================================
            // RETURN JWT
            // ==========================================

            return Ok(new
            {
                message = "Registration successful.",
                token = jwt,
                username = user.Username,
                isProfileComplete = user.IsProfileComplete
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request) {
            var user = await _context.UserLogin.FirstOrDefaultAsync(x => x.Username == request.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var passwordHasher = new PasswordHasher<UserLoginModel>();

            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.Password!,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password.");
            }

            // =========================
            // CREATE JWT
            // =========================

            var claims = new[] //impormasyon ito tungkol sa logged-in user.
            {
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim("Rowstamp", user.Rowstamp.ToString()),
                new Claim("IsProfileComplete", user.IsProfileComplete.ToString())
            };

            var key = new SymmetricSecurityKey( //FitNanceSuperSecretKey2026SomethingVeryLong
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]) // "ExpiresInMinutes": 60
                ),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwt,
                username = user.Username,
                isProfileComplete = user.IsProfileComplete
            });


            //return Ok(new
            //{
            //    message = "Login successful.",
            //    username = user.Username,
            //    isProfileComplete = user.IsProfileComplete
            //});


        }


        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            //return Ok(new
            //{
            //    message = "You are authenticated!"
            //});

            var username = User.Identity?.Name;

            return Ok(new
            {
                username = username
            });

        }



    }
}
