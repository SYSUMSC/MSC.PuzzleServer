using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class RecoveryModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string GToken { get; set; }
    }
}
