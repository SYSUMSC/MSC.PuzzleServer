using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models
{
    [Index(nameof(UserId), nameof(PuzzleId))]
    [JsonObject(MemberSerialization.OptIn)]
    public class Submission
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 提交的答案字符串
        /// </summary>
        [MaxLength(50)]
        [JsonProperty("answer")]
        public string Answer { get; set; } = string.Empty;

        /// <summary>
        /// 提交的答案是否正确
        /// </summary>
        [JsonProperty("solved")]
        public bool Solved { get; set; } = false;

        /// <summary>
        /// 提交的得分
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; } = 0;

        /// <summary>
        /// 答案提交的时间
        /// </summary>
        public DateTime SubmitTimeUTC { get; set; } = DateTime.Parse("1970-01-01T00:00:00");

        [NotMapped]
        [JsonProperty("time")]
        public string SubmitTime
        {
            get => SubmitTimeUTC.ToLocalTime().ToString("M/d HH:mm:ss");
        }

        #region 数据库关系
        /// <summary>
        /// 用户数据库Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public UserInfo User { get; set; }

        /// <summary>
        /// 谜题数据库Id
        /// </summary>
        public int PuzzleId { get; set; }

        /// <summary>
        /// 谜题
        /// </summary>
        public Puzzle Puzzle { get; set; }
        #endregion
    }
}
