using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models.Request
{
    public class RankMessageModel
    {
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("time")]
        public string UpdateTime { get; set; }
        [JsonProperty("name")]
        public string UserName { get; set; }
        [JsonProperty("descr")]
        public string Descr { get; set; }
    }
}
