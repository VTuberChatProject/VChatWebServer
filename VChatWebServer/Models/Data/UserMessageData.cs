using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class UserMessageData : DataCommon
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 是否为图片消息
        /// </summary>
        [JsonPropertyName("isImageMsg")]
        public bool IsImageMsg { get; set; } = false;

        /// <summary>
        /// 图片URL
        /// </summary>
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}

