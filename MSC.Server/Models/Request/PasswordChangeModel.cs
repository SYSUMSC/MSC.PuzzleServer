using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class PasswordChangeModel
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Old { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [MinLength(6)]
        public string New { get; set; }
    }
}
