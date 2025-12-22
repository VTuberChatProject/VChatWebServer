using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class DanmakuData : DataCommon
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("medalName")]
        public string MedalName { get; set; }

        [JsonPropertyName("medalLevel")]
        public int MedalLevel { get; set; }
    }
}

