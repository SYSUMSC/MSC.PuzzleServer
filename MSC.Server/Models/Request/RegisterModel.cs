using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class RegisterModel
    {
        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string GToken { get; set; }
    }
}
