using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

/// <summary>
/// 个人信息更改
/// </summary>
public class ProfileUpdateModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [MinLength(6)]
    [MaxLength(25)]
    public string? UserName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Descr { get; set; }

    /// <summary>
    /// 学号
    /// </summary>
    [RegularExpression("^[0-9]{8}$")]
    public string? StudentId { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Phone]
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; } = string.Empty;
}
