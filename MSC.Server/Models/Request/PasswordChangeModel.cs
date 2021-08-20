using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 密码更改
/// </summary>
public class PasswordChangeModel
{
    /// <summary>
    /// 旧密码
    /// </summary>
    [Required]
    [MinLength(6)]
    public string? Old { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Required]
    [MinLength(6)]
    public string? New { get; set; }
}
