using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class MailChangeModel
    {
        [Required]
        [EmailAddress]
        public string NewMail { get; set; }
    }
}
