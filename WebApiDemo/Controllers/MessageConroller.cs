using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MessageConroller : ControllerBase
    {
        private readonly ILogger<MessageConroller> _logger;
        public readonly Services.IChatService chatService;
        private readonly IHubContext<ChatHub> _hubContext;
        public MessageConroller(ILogger<MessageConroller> logger, Services.IChatService services)
        {
            _logger = logger;
            chatService = services;
        }

        [HttpPost("AddMessage")]
        public IActionResult AddMessage(string content)
        {
            ChatMessage newMessage = new ChatMessage()
            {
                Content = content
            };
            chatService.AddMessage(newMessage);
            _hubContext.Clients.All.SendAsync("ReceiveMessage", newMessage);
            return Ok(newMessage);

        }
    }
}