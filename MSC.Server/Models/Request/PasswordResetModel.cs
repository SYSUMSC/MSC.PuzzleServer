using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 密码重置
/// </summary>
public class PasswordResetModel
{
    /// <summary>
    /// 新密码
    /// </summary>
    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required]
    public string? Email { get; set; }

    /// <summary>
    /// 邮箱接收到的Base64格式Token
    /// </summary>
    [Required]
    public string? RToken { get; set; }

    /// <summary>
    /// 谷歌验证码
    /// </summary>
    public string? GToken { get; set; }
}
