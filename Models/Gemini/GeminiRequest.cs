using System.Text.Json.Serialization;

namespace Goksel_Chat_BotAPI.Models.Gemini;

// Bu sınıflar sadece Gemini API yapısına uymak için yazıldı.
// İç içe yazma nedenim JSON dizisi öyle istiyor.

public class GeminiRequest
{
    [JsonPropertyName("contents")]
    public List<Content> Contents { get; set; } = new ();
}

public class Content
{
    [JsonPropertyName("parts")] public List<Part> Parts { get; set; } = new();
}

public class Part
{
    [JsonPropertyName("text")] 
    public string Text { get; set; } = string.Empty;
}