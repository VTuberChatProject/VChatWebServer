using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VChatWebServer.Models.Data
{
    public class OperaData : DataCommon
    {
        [JsonPropertyName("opera")]
        public string Opera { get; set; }
    }
}
