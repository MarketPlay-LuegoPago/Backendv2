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
            // Obtiene el token de la configuración de la aplicación
            _apiToken = configuration["MailerSend:ApiToken"];

            // Configura el cliente HTTP con el token de autorización y la base URL de la API de MailerSend
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiToken}");
            _httpClient.BaseAddress = new Uri("https://api.mailersend.com/v1/");
        }

        public async Task SendEmailAsync(string from, string fromName, List<string> to, List<string> toNames, string subject, string text, string html)
        {
            // Configura los datos del correo electrónico
            var emailData = new
            {
                from = new { email = from, name = fromName },
                to = to.ConvertAll(email => new { email, name = toNames[to.IndexOf(email)] }),
                subject,
                text,
                html
            };

            // Serializa los datos del correo electrónico a formato JSON
            var jsonContent = JsonSerializer.Serialize(emailData);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Realiza la solicitud POST para enviar el correo electrónico
            var response = await _httpClient.PostAsync("email", content);

            // Verifica si la solicitud fue exitosa; de lo contrario, lanza una excepción
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error sending email: {response.ReasonPhrase}");
            }
        }
    }
}
