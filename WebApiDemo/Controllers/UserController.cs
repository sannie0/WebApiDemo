using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController: ControllerBase
    {
        //private readonly IHubContext<ChatHub> _hubContext;
        
        /*public UserController(IHubClients<ChatHub> hubContext)
        {
            _hubContext = (IHubContext<ChatHub>?)hubContext;
        }*/
        private readonly ILogger<UserController> _logger;
        private readonly IChatService _chatService;
        
        private readonly Services.IChatService chatService;
        public UserController(ILogger<UserController> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }

        /*private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));// преобразование пароля в байтовый массив

                StringBuilder builder = new StringBuilder();// преобразование байтового массива в строку HEX
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }*/
        private string Generate(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUserAsync(string username, string password)
        {
            try
            {
                User newUser = new()
                {
                    UserId = 0,
                    UserName = username,
                    Password = password
                };
                //newUser.UserId = _chatService.GetUserId(newUser.UserName);
                string token = _chatService.LoginUser(newUser);
                if (token == "-1")
                {
                    return BadRequest("User not found!");
                }
                if (token == "-2")
                {
                    return BadRequest("Password is wrong");
                }
                HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });
                //string result = $"{token} {user}"
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while registration user: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult RegisterUser(string username, string password)
        {
            try
            {
                if (username == null || password == null)
                {
                    return BadRequest("Invalid data received from client.");
                }
                var newUser = new User
                {
                    UserId = _chatService.GetUserId(),
                    UserName = username,
                    Password = password//Generate()
                };

                var result = _chatService.Registration(newUser);
                if (result == 1)
                {
                    _logger.LogInformation("Данные получены!");
                    return Ok(newUser);
                }
                return StatusCode(500, "User is already registered");
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while registration user: {ex.Message}");
                return StatusCode(500, "The user is already registered");
            }
        }

        private bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKey");
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true; 
            }
            catch
            {
                return false;
            }
        }
        
        [HttpPost]
        public IActionResult SomeAction()
        {
            string token = HttpContext.Request.Cookies["jwt"];

            bool isValidToken = ValidateToken(token);

            if (isValidToken)
            {
                return Ok("Valid token");
            }
            else
            {
                return BadRequest("Invalid token");
            }
        }
    }
}
