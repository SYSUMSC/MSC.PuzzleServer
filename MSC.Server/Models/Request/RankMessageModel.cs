using Newtonsoft.Json;

namespace MSC.Server.Models.Request
{
    /// <summary>
    /// 排名信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RankMessageModel
    {
        /// <summary>
        /// 当前得分
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("time")]
        public string? UpdateTime { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("name")]
        public string? UserName { get; set; }

        /// <summary>
        /// 用户描述
        /// </summary>
        [JsonProperty("descr")]
        public string? Descr { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId { get; set; }
    }
}