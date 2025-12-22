using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Common
{
    public class FromInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}

