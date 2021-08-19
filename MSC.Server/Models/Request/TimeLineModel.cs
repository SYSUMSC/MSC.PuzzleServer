using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models.Request
{
    /// <summary>
    /// 时间轴
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TimeLineModel
    {
        /// <summary>
        /// 解出的题目Id
        /// </summary>
        public int PuzzleId { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }
        /// <summary>
        /// 当前总分值
        /// </summary>
        [JsonProperty("score")]
        public int TotalScore { get; set; }

        public override int GetHashCode()
        {
            return PuzzleId;
        }
    }
}
