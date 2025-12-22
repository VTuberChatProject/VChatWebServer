using System.Text.Json.Serialization;
using VChatWebServer.Models.Common;

namespace VChatWebServer.Models.Data
{
    public class DataCommon
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        [JsonPropertyName("right")]
        public bool Right { get; set; }

        [JsonPropertyName("user")]
        public UserInfo User { get; set; }
    }
}

