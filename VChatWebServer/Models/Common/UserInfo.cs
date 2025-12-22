using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Common
{
    public class UserInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}

