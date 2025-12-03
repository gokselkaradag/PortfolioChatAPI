using Goksel_Chat_BotAPI.Models;
using Goksel_Chat_BotAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goksel_Chat_BotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IGeminiService _geminiService;

        public ChatController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }
        
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] UserRequesDto request)
        {
            // Boş Gelen Veri Uyarı !!!
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new ChatResponseDto
                {
                    IsSuccess = false,
                    Reply = "Lütfen boş mesaj göndermeyin."
                });
            }
            
            // Controller servis çağrısı
            var result = await _geminiService.GetAnswerFromGeminiAsync(request.Message);
            
            return Ok(result);
        }
    }
}
