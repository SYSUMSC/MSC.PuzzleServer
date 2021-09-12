﻿using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 注册账号
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名是必需的")]
    [MinLength(6, ErrorMessage = "用户名过短")]
    [MaxLength(25, ErrorMessage = "用户名过长")]
    public string? UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码是必需的")]
    public string? Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱是必需的")]
    [EmailAddress(ErrorMessage = "邮箱地址无效")]
    public string? Email { get; set; }
}
