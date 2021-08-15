using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class RecoveryModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "无效的电子邮件地址。")]
        public string Email { get; set; }
        public string GToken { get; set; }
    }
}
