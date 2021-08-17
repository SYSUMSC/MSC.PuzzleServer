using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models
{
    public class Puzzle
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 谜题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 谜题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 谜题答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 访问等级
        /// </summary>
        public int AccessLevel { get; set; } = 0;

        /// <summary>
        /// 解决谜题人数
        /// </summary>
        public int SolvedCount { get; set; } = 0;

        /// <summary>
        /// 初始分数
        /// </summary>
        public int OriginalScore { get; set; } = 10000;

        /// <summary>
        /// 最低分数
        /// </summary>
        public int MinScore { get; set; } = 3000;

        /// <summary>
        /// 预期最大解出人数
        /// </summary>
        public int ExpectMaxCount { get; set; } = 100;

        /// <summary>
        /// 奖励人数
        /// </summary>
        public int AwardCount { get; set; } = 10;

        /// <summary>
        /// 当前题目分值
        /// </summary>
        [NotMapped]
        public int CurrentScore
        {
            get
            {
                if (SolvedCount <= AwardCount)
                    return OriginalScore - SolvedCount;
                if (SolvedCount > ExpectMaxCount)
                    return MinScore;
                var range = OriginalScore - AwardCount - MinScore;
                return (int)(OriginalScore - AwardCount 
                    - Math.Floor( range * (SolvedCount - AwardCount)/(float)(ExpectMaxCount - AwardCount)));
            }
        }

        #region 数据库关系
        public List<Process> Processes { get; set; } = new();

        public List<Submission> Submissions { get; set; } = new();
        #endregion
    }
}
