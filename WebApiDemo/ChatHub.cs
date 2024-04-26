using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using WebApiDemo.Controllers;
using WebApiDemo.Services;

namespace WebApiDemo
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> logger;
        private readonly IChatService _chatService;

        public ChatHub(ILogger<ChatHub> logger, IChatService chatService)
        {
            this.logger = logger;
            _chatService = chatService;
        }


        public async Task SendMessageAll(ChatMessage message)
        {
            string userName = _chatService.GetUserName(message.UserId);
            if (userName != "Unknown User")
            {
                string newMessage = $"{userName}: {message.Content}"; // 
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
        }

        /*public async Task ReceiveMessage(ChatMessage message, string user_id)
        {
            var userName = _chatService.GetUserName(int.Parse(user_id));
            var new_message = $"{userName}: {message.Content}";
            await Clients.All.SendAsync("ReceiveMessage", new_message);
        }*/

        
        
    }
}
