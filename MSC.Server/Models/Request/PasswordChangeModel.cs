using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class PasswordChangeModel
    {
        [Required]
        [MinLength(6)]
        public string Old { get; set; }
        [Required]
        [MinLength(6)]
        public string New { get; set; }
    }
}
