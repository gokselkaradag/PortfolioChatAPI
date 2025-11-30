using Goksel_Chat_BotAPI.Models;

namespace Goksel_Chat_BotAPI.Services;

public interface IGeminiService
{
    Task<ChatResponseDto> GetAnswerFromGeminiAsync(string userMessage);
}