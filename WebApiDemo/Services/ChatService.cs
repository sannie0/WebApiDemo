using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using WebApiDemo.Controllers;

namespace WebApiDemo.Services
{
    public class ChatService: IChatService
    {
        private readonly List<User> users = new List<User>();
        private readonly JwtOptions _jwtOptions;
        private readonly IHubContext<ChatHub> _hubContext;

 
        private List<ChatInterface> chats;
        

        private string usersFilePath = "WebApiDemo/WebApiDemo/user.json";

        public ChatService(IOptions<JwtOptions> jwtOptions)//IHubContext<ChatHub> hubContext, 
        {
            _jwtOptions = jwtOptions.Value;
            //_hubContext = hubContext;
        }

        public string GenerateToken(User user)
        {
            Claim[] claims = new[] { new Claim("userId", user.UserId.ToString()) };
                
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));
            
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            
            return tokenValue;
        }
        
        public int GetUserId()
        {
            if (users == null) return 0;
            return users.Count();
        }

        public void SaveUsers()
        {
            var Userjson = JsonConvert.SerializeObject(users);
            File.WriteAllText("user.json", Userjson);
        }

        public void Registration(User newUser)
        {
            users.Add(newUser);
            SaveUsers();
        }
        
        private bool Verify(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
        
        public string LoginUser(User user)
        {
            User RegUser = users.FirstOrDefault(u => u.UserName == user.UserName);
            if (RegUser == null)
            {
                return "-1";
            }
            if (RegUser.Password != user.Password)//Verify(RegUser.Password, user.Password) == false
            {
                return "-2"; 
            }

            var token = GenerateToken(RegUser);
            
            return token; 
        }
    }
}
