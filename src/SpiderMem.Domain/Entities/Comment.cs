namespace SpiderMem.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid MemeId { get; set; }
    public Meme Meme { get; set; } = null!;
}
