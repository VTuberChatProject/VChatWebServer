using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class NewVipData : DataCommon
    {
        [JsonPropertyName("price")]
        public long Price { get; set; }
    }
}

