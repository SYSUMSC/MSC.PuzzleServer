using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSC.Server.Models;

[Index(nameof(UserId), nameof(PuzzleId))]
public class Submission
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// 提交的答案字符串
    /// </summary>
    [MaxLength(50)]
    [JsonPropertyName("answer")]
    public string Answer { get; set; } = string.Empty;

    /// <summary>
    /// 提交的答案是否正确
    /// </summary>
    [JsonPropertyName("solved")]
    public bool Solved { get; set; } = false;

    /// <summary>
    /// 提交的得分
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; } = 0;

    /// <summary>
    /// 答案提交的时间
    /// </summary>
    [JsonIgnore]
    public DateTime SubmitTimeUTC { get; set; } = DateTime.Parse("1970-01-01T00:00:00");

    [NotMapped]
    [JsonPropertyName("time")]
    public string SubmitTime
    {
        get => SubmitTimeUTC.ToLocalTime().ToString("M/d HH:mm:ss");
    }

    #region 数据库关系

    /// <summary>
    /// 用户数据库Id
    /// </summary>
    [JsonIgnore]
    public string? UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [JsonIgnore]
    public UserInfo? User { get; set; }

    /// <summary>
    /// 谜题数据库Id
    /// </summary>
    [JsonIgnore]
    public int PuzzleId { get; set; }

    /// <summary>
    /// 谜题
    /// </summary>
    [JsonIgnore]
    public Puzzle? Puzzle { get; set; }

    #endregion 数据库关系
}
