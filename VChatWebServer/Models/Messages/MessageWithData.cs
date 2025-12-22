using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Messages
{
    public class MessageWithData<TData> : MessageBase
    {
        [JsonPropertyName("data")]
        public TData Data { get; set; }
    }
}

