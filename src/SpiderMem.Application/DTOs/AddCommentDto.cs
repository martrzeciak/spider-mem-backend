namespace SpiderMem.Application.DTOs;

public record AddCommentDto{
    public Guid MemeId { get; set; }
    public string Content { get; set; } = string.Empty;
}
