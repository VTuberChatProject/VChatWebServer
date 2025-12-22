using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class SystemData
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        [JsonPropertyName("right")]
        public bool Right { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}

