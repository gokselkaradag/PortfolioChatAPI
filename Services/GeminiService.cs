using System.Text;
using System.Text.Json;
using Goksel_Chat_BotAPI.Models;
using Goksel_Chat_BotAPI.Models.Gemini;

namespace Goksel_Chat_BotAPI.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    // Artık const değil, readonly bir değişken çünkü dosyadan okuyacağız.
    private readonly string _systemPrompt;
    
    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        
        // Uygulamanın çalıştığı dizini buluyoruz
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(baseDir, "system-prompt.txt");

        if (File.Exists(filePath))
        {
            _systemPrompt = File.ReadAllText(filePath);
        }
        else
        {
            // Dosya yoksa (örn: GitHub'dan çeken biri unuttuysa) varsayılan basit bir prompt ata
            _systemPrompt = "Sen yararlı bir asistansın.";
        }
    }

    public async Task<ChatResponseDto> GetAnswerFromGeminiAsync(string userMessage)
    {
        // Ayarları alıyoruz.
        var baseUrl = _configuration["GeminiSettings:BaseUrl"];

        // API Key'i entegre ediyoruz.
        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        // URL'yi oluştur
                var requestUrl = $"{baseUrl}?key={apiKey}";
        
                // 3. İsteği Hazırla
                var fullPrompt = $"{_systemPrompt}\n\nKullanıcı: {userMessage}";
                
                var geminiRequest = new GeminiRequest();
                geminiRequest.Contents.Add(new Content
                {
                    Parts = new List<Part> { new Part { Text = fullPrompt } }
                });
        
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(geminiRequest), 
                    Encoding.UTF8, 
                    "application/json");
        
                try
                {
                    // 4. POST İsteği Gönder (Artık 2.0-flash modeline gidiyor)
                    var response = await _httpClient.PostAsync(requestUrl, jsonContent);
                    var responseString = await response.Content.ReadAsStringAsync();
        
                    if (!response.IsSuccessStatusCode)
                    {
                        return new ChatResponseDto 
                        { 
                            IsSuccess = false, 
                            Reply = $"HATA ({response.StatusCode}): {responseString}" 
                        };
                    }
        
                    // 5. Cevabı İşle
                    using (JsonDocument doc = JsonDocument.Parse(responseString))
                    {
                        var root = doc.RootElement;
                        if (root.TryGetProperty("candidates", out JsonElement candidates) && candidates.GetArrayLength() > 0)
                        {
                            var text = candidates[0]
                                .GetProperty("content")
                                .GetProperty("parts")[0]
                                .GetProperty("text")
                                .GetString();
        
                            return new ChatResponseDto { IsSuccess = true, Reply = text };
                        }
                    }
        
                    return new ChatResponseDto { IsSuccess = false, Reply = "Cevap boş döndü." };
                }
                catch (Exception ex)
                {
                    return new ChatResponseDto { IsSuccess = false, Reply = $"Kod Hatası: {ex.Message}" };
                }
    }
}