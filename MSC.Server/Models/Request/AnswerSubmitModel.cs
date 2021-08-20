using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    /// <summary>
    /// 答案提交
    /// </summary>
    public class AnswerSubmitModel
    {
        /// <summary>
        /// 谜题答案
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [RegularExpression("^msc{[a-zA-Z0-9_-]+}$")]
        public string Answer { get; set; } = string.Empty;
    }
}