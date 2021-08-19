using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [MaxLength(30)]
        [RegularExpression("^msc{[a-zA-Z0-9_-]+}$")]
        public string Answer { get; set; }
    }
}
