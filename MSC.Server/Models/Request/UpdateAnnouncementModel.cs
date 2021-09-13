using System.ComponentModel.DataAnnotations;

namespace MSC.Server.Models.Request;

public class UpdateAnnouncementModel
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public bool? IsPinned { get; set; }
}
