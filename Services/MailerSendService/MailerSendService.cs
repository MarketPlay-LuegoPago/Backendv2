using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Backengv2.Services
{
    public class MailerSendService
    {
        private readonly string _apiToken;
        private readonly HttpClient _httpClient;

        public MailerSendService(IConfiguration configuration)
        {
            _apiToken = configuration["MailerSend:ApiToken"];
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiToken}");
            _httpClient.BaseAddress = new Uri("https://api.mailersend.com/v1/");
        }

        public async Task SendEmailAsync(string from, string fromName, List<string> to, List<string> toNames, string subject, string text, string html)
        {
            var emailData = new
            {
                from = new { email = from, name = fromName },
                to = to.ConvertAll(email => new { email, name = toNames[to.IndexOf(email)] }),
                subject,
                text,
                html
            };

            var jsonContent = JsonSerializer.Serialize(emailData);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("email", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error sending email: {response.ReasonPhrase}");
            }
        }
    }
}
