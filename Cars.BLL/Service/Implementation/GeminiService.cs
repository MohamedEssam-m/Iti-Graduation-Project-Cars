using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Cars.BLL.Service.Implementation
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IConfiguration configuration;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            
            _apiKey = configuration["Gemini:ApiKey"];


            
        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            try
            {
                var requestUrl = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-pro:generateContent?key={_apiKey}";



                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = userMessage }
                            }
                        }
                    }
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: {response.StatusCode} - {responseString}";
                }

                var jsonResponse = JObject.Parse(responseString);
                var generatedText = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                return generatedText ?? "No response generated.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<List<string>> GetAvailableModelsAsync()
        {
            var url = $"https://generativelanguage.googleapis.com/v1/models?key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"Gemini API Error ({response.StatusCode}): {json}");
            }

            // Parse JSON
            var result = JsonDocument.Parse(json);
            var models = new List<string>();

            if (result.RootElement.TryGetProperty("models", out var modelsArray))
            {
                foreach (var model in modelsArray.EnumerateArray())
                {
                    if (model.TryGetProperty("name", out var modelName))
                    {
                        models.Add(modelName.GetString());
                    }
                }
            }

            return models;
        }

    }
}
