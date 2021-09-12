﻿using System.Text.Json.Serialization;

namespace MSC.Server.Models.Request;

/// <summary>
/// 排名信息
/// </summary>
public class RankMessageModel
{
    /// <summary>
    /// 当前得分
    /// </summary>
    [JsonPropertyName("score")]
    public int Score { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("time")]
    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [JsonPropertyName("name")]
    public string? UserName { get; set; }

    /// <summary>
    /// 用户描述
    /// </summary>
    [JsonPropertyName("descr")]
    public string? Descr { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [JsonIgnore]
    public string? UserId { get; set; }
}
