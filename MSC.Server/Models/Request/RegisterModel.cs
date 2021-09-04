using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 注册账号
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [MinLength(6)]
    [MaxLength(25)]
    public string? UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
