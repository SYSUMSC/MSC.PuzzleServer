using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MSC.Server.Models;

public class PuzzleBase
{
    /// <summary>
    /// 谜题名称
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 谜题内容
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 谜题答案
    /// </summary>
    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [RegularExpression("^msc{[a-zA-Z0-9_-]+}$")]
    public string Answer { get; set; } = string.Empty;

    /// <summary>
    /// 访问等级
    /// </summary>
    [Required]
    public int AccessLevel { get; set; } = 0;

    /// <summary>
    /// 解决谜题人数
    /// </summary>
    [Required]
    [JsonIgnore]
    public int AcceptedCount { get; set; } = 0;

    /// <summary>
    /// 提交答案的人数
    /// </summary>
    [Required]
    [JsonIgnore]
    public int SubmissionCount { get; set; } = 0;

    /// <summary>
    /// 初始分数
    /// </summary>
    [Required]
    public int OriginalScore { get; set; } = 10000;

    /// <summary>
    /// 最低分数
    /// </summary>
    [Required]
    public int MinScore { get; set; } = 3000;

    /// <summary>
    /// 预期最大解出人数
    /// </summary>
    [Required]
    public int ExpectMaxCount { get; set; } = 100;

    /// <summary>
    /// 奖励人数
    /// </summary>
    [Required]
    public int AwardCount { get; set; } = 10;

    /// <summary>
    /// 升级访问权限（为0则不变）
    /// </summary>
    [Required]
    public int UpgradeAccessLevel { get; set; } = 0;
}
