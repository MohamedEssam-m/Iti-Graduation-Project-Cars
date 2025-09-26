using System.Text.Json.Serialization;
namespace Cars.BLL.ModelVM.OpenAi
{
    public class OpenAiResponse
    {
        [JsonPropertyName("text")]
        public string text {  get; set; }
    }
}
