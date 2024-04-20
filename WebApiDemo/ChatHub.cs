using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace WebApiDemo
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> logger;


        public ChatHub(ILogger<ChatHub> logger)
        {
            this.logger = logger;
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }
    }
}
