using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class AccountVerifyModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
