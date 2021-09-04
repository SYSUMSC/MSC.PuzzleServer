using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 找回账号
/// </summary>
public class RecoveryModel
{
    /// <summary>
    /// 用户邮箱
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
