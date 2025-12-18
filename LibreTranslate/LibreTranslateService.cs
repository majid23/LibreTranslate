using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibreTranslate
{
    public class LibreTranslateService : ILibreTranslateService
    {
        private readonly LibreTranslateConfig _config;
        private readonly HttpClient _httpClient;

        public LibreTranslateService(IOptions<LibreTranslateConfig> config, HttpClient httpClient)
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<string> TranslateAsync(string text, string source, string target)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(_config.ApiAddress))
                return text;

            var body = new
            {
                q = text,
                source = source,
                target = target,
                format = "text",
                api_key = _config.ApiKey
            };

            try
            {
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_config.ApiAddress, content);
                response.EnsureSuccessStatusCode();

                var resultJson = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TranslationResult>(resultJson);
                return result?.TranslatedText ?? text;
            }
            catch
            {
                // اگر خطایی در پردازش JSON رخ داد، متن اصلی را برگردانید.
                return text;
            }

            //using var doc = JsonDocument.Parse(resultJson);
            //if (doc.RootElement.TryGetProperty("translatedText", out var translated))
            //{
            //    return translated.GetString() ?? text;
            //}

            //return text;
        }

        public class TranslationResult
        {
            public string TranslatedText { get; set; }
        }
    }
}
