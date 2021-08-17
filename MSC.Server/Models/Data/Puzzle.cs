using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        #region 数据库关系
        public List<Process> Processes { get; set; } = new();

        public List<Submission> Submissions { get; set; } = new();
        #endregion
    }
}
