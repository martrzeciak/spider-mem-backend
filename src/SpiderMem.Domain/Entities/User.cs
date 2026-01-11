using Microsoft.AspNetCore.Identity;

namespace SpiderMem.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Meme> Memes { get; set; } = new List<Meme>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}
