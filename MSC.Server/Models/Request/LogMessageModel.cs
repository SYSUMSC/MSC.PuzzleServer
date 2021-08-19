using Newtonsoft.Json;

namespace MSC.Server.Models
{
    public class LogMessageModel
    {
        /// <summary>
        /// 日志时间
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("name")]
        public string UserName { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [JsonProperty("ip")]
        public string IP { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        [JsonProperty("msg")]
        public string Msg { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
