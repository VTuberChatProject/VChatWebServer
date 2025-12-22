using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class GiftData : DataCommon
    {
        [JsonPropertyName("giftname")]
        public string GiftName { get; set; }

        [JsonPropertyName("num")]
        public int Num { get; set; }
    }
}

