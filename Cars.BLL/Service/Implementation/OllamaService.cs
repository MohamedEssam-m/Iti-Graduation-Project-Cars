using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Cars.BLL.Service.Implementation
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AskModelAsync(string prompt, bool useStream = false)
        {
            var requestBody = new
            {
                model = "phi",
                prompt = prompt,
                stream = useStream 
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            if (!useStream)
            {
                
                
                var response = await _httpClient.PostAsync("/api/generate", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("response", out var resp))
                {
                    return resp.GetString() ?? string.Empty;
                }

                return string.Empty;
            }
            else
            {
                
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/generate")
                {
                    Content = content
                };

                var response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead
                );

                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var sb = new StringBuilder();

                while (true)
                {
                    var line = await reader.ReadLineAsync();
                    if (line == null)
                        break;

                    if (line.StartsWith("data:"))
                    {
                        line = line.Substring(5).Trim();
                    }

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        using var doc = JsonDocument.Parse(line);
                        if (doc.RootElement.TryGetProperty("response", out var resp))
                        {
                            var text = resp.GetString();
                            if (!string.IsNullOrEmpty(text))
                            {
                                sb.Append(text);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line: {ex.Message}");
                    }
                }

                return sb.ToString().Trim();
            }
        }
    }
}
