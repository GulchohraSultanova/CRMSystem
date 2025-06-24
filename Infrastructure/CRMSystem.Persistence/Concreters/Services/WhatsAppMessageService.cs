using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Message;
using System.Text;

public class WhatsAppMessageService : IWhatsAppMessageService
{
    private readonly HttpClient _httpClient;
    private readonly string _token = "EAATSlTSki7IBO1ZBPZCfJUJsk01LH36kTGmpZBZC993KZBPZAYEmzWfqmWXJmlsdwFicGyzBv4SefwZARR5N6o3vDSTq3P3CW29bAJtYFuYRmP4osBlgx6deDAIrDp3ZBL7KlPZCUo20dwqgYhRwUuiYgwyyVufCZA7c5yO6msaiCJknWCyHTVMcTll4NolNVT4R3sorSeisRxWLSqLX4qXsvbGVnwm7SoQPGlmheXr7lcIuOfSR5uNAeZAZBIw9jESvmQZDZD";
    private readonly string _phoneNumberId = "711819008675793";

    public WhatsAppMessageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendMessageAsync(WhatsAppMessageDto dto)
    {
        var url = $"https://graph.facebook.com/v19.0/{_phoneNumberId}/messages";
        var payload = new
        {
            messaging_product = "whatsapp",
            to = dto.PhoneNumber,
            type = "template",
            template = new
            {
                name = "hello_world",
                language = new { code = "en_US" }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine("WhatsApp Response: " + responseContent);
        response.EnsureSuccessStatusCode();
    }
}
