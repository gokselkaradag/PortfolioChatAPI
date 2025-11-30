using System.Text;
using System.Text.Json; // JSON işlemleri için
using Goksel_Chat_BotAPI.Models;
using Goksel_Chat_BotAPI.Models.Gemini;

namespace Goksel_Chat_BotAPI.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    private const string SystemPrompt = @"
        Sen Göksel Karadağ'ın kişisel web sitesi asistanısın.
        Adın: GökselAI.
        Görevin: Ziyaretçilerin Göksel hakkındaki sorularını yanıtlamak.
        
        Göksel Hakkında Bilgiler:
        - Pozisyon: Junior Backend Developer.
        - Uzmanlık: C#, .NET Core, Web API.
        - İlgi Alanları: React Native, Generative AI, Mobil Uygulama Geliştirme.
        - Projeler: 'GifRise' adında AI tabanlı GIF uygulaması geliştirdi.
        - İletişim: info@gokselkaradag.com.tr
        
        Kurallar:
        1. Cevapların samimi, kısa ve Türkçe olsun.
        2. Bilmediğin bir şey sorulursa uydurma, 'Bunu Göksel'e sormam lazım' de.
    ";

    public async Task<ChatResponseDto> GetAnswerFromGeminiAsync(string userMessage)
    {
        // Ayarları alıyoruz.
        var baseUrl = _configuration["GeminiSettings:BaseUrl"];

        // API Key'i entegre ediyoruz.
        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        // Güvenlik kontrolü
        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey))
        {
            return new ChatResponseDto { IsSuccess = false, Reply = "Sunucu yapılandırma hatası: API Key bulunamadı." };
        }

        // Gemini'ye gidicek mesaj.
        // Kullanıcıya sunulacak promptu ekliyorum.
        var fullPrompt = $"{SystemPrompt}\n\nKullanıcı Sorusu: {userMessage}";

        var geminiRequest = new GeminiRequest();
        geminiRequest.Contents.Add(new Content
        {
            Parts = new List<Part> { new Part { Text = fullPrompt } }
        });
        
        // Text'i Json'a çevir.
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(geminiRequest),
            Encoding.UTF8,
            "application/json");
        
        // Url kısmına apikey ekleniyor.
        var requestUrl = $"{baseUrl}?key={apiKey}";

        try
        {
            // İstek Gönderimi
            var response = await _httpClient.PostAsync(requestUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                // Hata log ekranı
                return new ChatResponseDto{ IsSuccess = false, Reply = "Yapay zeka servisine ulaşılamadı." };
            }
            
            // Cevabı okuyup işlemeye başlıyor 
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Gelen Json'u parse ediyorum.
            using (JsonDocument doc = JsonDocument.Parse(responseString))
            {
                // Json path ile cevaba ulaşıyorum.
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out JsonElement candidates) && candidates.GetArrayLength() > 0)
                {
                    var text = candidates[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                    
                    return new  ChatResponseDto { IsSuccess = true, Reply = text };
                }
            }

            return new ChatResponseDto { IsSuccess = false, Reply = "Anlamlı bir cevap alınamadı. " };
        }
        catch (Exception ex)
        {
            return new ChatResponseDto { IsSuccess = false, Reply = $"Hata: {ex.Message}" };
        }
    }
}