﻿using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 密码重置
/// </summary>
public class PasswordResetModel
{
    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码是必需的")]
    [MinLength(6, ErrorMessage = "密码过短")]
    public string? Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱是必需的")]
    public string? Email { get; set; }

    /// <summary>
    /// 邮箱接收到的Base64格式Token
    /// </summary>
    [Required(ErrorMessage = "Token是必需的")]
    public string? RToken { get; set; }
}
