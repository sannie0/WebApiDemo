using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MessageConroller : ControllerBase
    {
        private readonly ILogger<MessageConroller> _logger;

        public MessageConroller(ILogger<MessageConroller> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}/{message}")]
        public IActionResult Get(int id, string message)
        {
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
    }
}