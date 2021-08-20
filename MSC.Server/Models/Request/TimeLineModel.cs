﻿using System.Text.Json.Serialization;

namespace MSC.Server.Models.Request;

/// <summary>
/// 时间轴
/// </summary>
public class TimeLineModel
{
    /// <summary>
    /// 解出的题目Id
    /// </summary>
    [JsonIgnore]
    public int PuzzleId { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    /// <summary>
    /// 当前总分值
    /// </summary>
    [JsonPropertyName("score")]
    public int TotalScore { get; set; }
}
