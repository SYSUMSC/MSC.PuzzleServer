﻿using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 邮箱更改
/// </summary>
public class MailChangeModel
{
    /// <summary>
    /// 新邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱是必需的")]
    [EmailAddress(ErrorMessage = "邮箱地址无效")]
    public string? NewMail { get; set; }
}
