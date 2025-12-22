using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class SuperChatData : DataCommon
    {
        [JsonPropertyName("price")]
        public long Price { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}

