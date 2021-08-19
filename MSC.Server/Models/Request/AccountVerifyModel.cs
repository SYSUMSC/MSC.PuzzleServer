using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class AccountVerifyModel
    {
        /// <summary>
        /// 邮箱接收到的Base64格式Token
        /// </summary>
        [Required]
        public string Token { get; set; }
        /// <summary>
        /// 用户邮箱的Base64格式
        /// </summary>
        [Required]
        public string Email { get; set; }
    }
}
