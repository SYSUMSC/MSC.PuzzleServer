using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models
{
    public class PuzzleBase
    {
        /// <summary>
        /// 谜题名称
        /// </summary>
        [Required]
        [MinLength(1)]
        public string ?Title { get; set; }

        /// <summary>
        /// 谜题内容
        /// </summary>
        [Required]
        public string ?Content { get; set; }

        /// <summary>
        /// 谜题答案
        /// </summary>
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        [RegularExpression("^msc{[a-zA-Z0-9_-]+}$")]
        public string ?Answer { get; set; }

        /// <summary>
        /// 访问等级
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int AccessLevel { get; set; } = 0;

        /// <summary>
        /// 解决谜题人数
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int SolvedCount { get; set; } = 0;

        /// <summary>
        /// 初始分数
        /// </summary>
        [Required]
        [DefaultValue(10000)]
        public int OriginalScore { get; set; } = 10000;

        /// <summary>
        /// 最低分数
        /// </summary>
        [Required]
        [DefaultValue(3000)]
        public int MinScore { get; set; } = 3000;

        /// <summary>
        /// 预期最大解出人数
        /// </summary>
        [Required]
        [DefaultValue(100)]
        public int ExpectMaxCount { get; set; } = 100;

        /// <summary>
        /// 奖励人数
        /// </summary>
        [Required]
        [DefaultValue(10)]
        public int AwardCount { get; set; } = 10;

        /// <summary>
        /// 升级访问权限（为0则不变）
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int UpgradeAccessLevel { get; set; } = 0;
    }
}