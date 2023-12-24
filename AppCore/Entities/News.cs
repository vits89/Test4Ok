using System.ComponentModel.DataAnnotations;

namespace Test4Ok.AppCore.Entities;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime? PublishDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    [Required]
    public NewsSource NewsSource { get; set; } = new();
}
