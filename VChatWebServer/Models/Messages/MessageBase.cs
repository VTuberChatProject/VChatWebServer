using System.Text.Json.Serialization;
using VChatWebServer.Serialization;
using VChatWebServer.Models.Common;

namespace VChatWebServer.Models.Messages
{
    [JsonConverter(typeof(MessageBaseJsonConverter))]
    public abstract class MessageBase
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("target")]
        public string Target { get; set; } = "@vchat_*";

        [JsonPropertyName("from")]
        public FromInfo From { get; set; }
    }
}
