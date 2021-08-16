using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request
{
    public class LogRequestModel
    {
        public int Skip { get; set; } = 0;
        [Range(1,50)]
        public int Count { get; set; } = 50;
        public string Level { get; set; } = "All";
    }
}
