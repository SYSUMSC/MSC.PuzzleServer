using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        /// <summary>
        /// 谷歌验证码
        /// </summary>
        public string GToken { get; set; }
    }
}
