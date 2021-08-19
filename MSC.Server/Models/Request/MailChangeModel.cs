using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
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
