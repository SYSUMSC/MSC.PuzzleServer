using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    /// <summary>
    /// 邮箱更改
    /// </summary>
    public class MailChangeModel
    {
        /// <summary>
        /// 新邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        public string NewMail { get; set; }
    }
}