using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class ProfileUpdateModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        public string UserName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Des { get; set; }
    }
}
