using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class PasswordResetModel
    {
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string RToken { get; set; }
        public string GToken { get; set; }
    }
}
