﻿using Newtonsoft.Json;

namespace MSC.Server.Models
{
    public class LogMessageModel
    {
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("name")]
        public string UserName { get; set; }
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("msg")]
        public string Msg { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}