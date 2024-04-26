using System.IdentityModel.Tokens.Jwt;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;



namespace WebApiDemo.Controllers

{
    [ApiController]
    [Route("/api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private List<ChatInterface> _chatRooms = new List<ChatInterface>();

        private readonly Services.IChatService _chatService;
        public ChatController(Services.IChatService services)
        {
            _chatService = services;
        }
        private int lastUserId;

        public ChatController(ILogger<ChatController> logger, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
        
        [HttpPost]
        [Route("AddUserToChat")]
        public async Task<IActionResult> JoinChat()
        {
            try
            {
                var token = Request.Cookies["jwt"];

                return Ok("Joined chat successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while joining chat: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        /*[HttpPost]
        [Route("CreateChatRoom")]
        public IActionResult CreateChatRoom([FromBody] ChatInterface model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogError("Received null model from client.");
                    return BadRequest("Invalid data received from client.");
                }

                // �������� ����� ������� ����
                var chatRoom = new ChatInterface
                {
                    ChatName = model.ChatName,
                    UserName = model.ChatName
                };

                _chatRooms.Add(chatRoom);// ���������� ������� � ��������� �����

                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating chatroom: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }*/





    }
}
