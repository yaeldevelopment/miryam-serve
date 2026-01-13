using System.Net;
using System.Net.Mail;
using System.Text;

namespace server.Models
{
    public class Helper
    {
        public static async Task SendEmailAsync( string subject, string html)
        {
            var apiKey = Environment.GetEnvironmentVariable("RESEND_API_KEY");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var payload = new
            {
                from = "onboarding@resend.dev",
                to = new[] {  Environment.GetEnvironmentVariable("to_email") },
                subject = subject,
                html = html
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://api.resend.com/emails", content);
            response.EnsureSuccessStatusCode();
        }

    }
}
