using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class ProfileUpdateModel
    {
        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        public string UserName { get; set; }
        public string Des { get; set; }
    }
}
