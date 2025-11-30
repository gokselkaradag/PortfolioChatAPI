namespace Goksel_Chat_BotAPI.Models;

public class ChatResponseDto
{
    // Bizim UI döneceğimiz cevap.
    public string? Reply { get; set; }
    
    // İşlem başarılı mı ? bilgisi dönecek, UI duruma göre hata mesajı döndürecek.
    public bool IsSuccess { get; set; }
}