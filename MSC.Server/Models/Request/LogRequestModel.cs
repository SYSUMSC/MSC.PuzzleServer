using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class LogRequestModel
    {
        [DefaultValue(0)]
        public int Skip { get; set; } = 0;
        [Range(1, 50)]
        [DefaultValue(50)]
        public int Count { get; set; } = 50;
        [DefaultValue("All")]
        public string Level { get; set; } = "All";
    }
}
