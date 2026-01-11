namespace SpiderMem.Domain.Entities;

public class Tag
{
    public string Name { get; set; } = null!;
    public ICollection<Meme> Memes { get; set; } = new List<Meme>();
}
