using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class CharData : DataCommon
    {
        [JsonPropertyName("char")]
        public string Char { get; set; }
    }
}
