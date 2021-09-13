﻿using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 个人信息更改
/// </summary>
public class ProfileUpdateModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MinLength(6, ErrorMessage = "用户名过短")]
    [MaxLength(25, ErrorMessage = "用户名过长")]
    public string? UserName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Descr { get; set; }

    /// <summary>
    /// 学号
    /// </summary>
    public string? StudentId { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; } = string.Empty;
}
