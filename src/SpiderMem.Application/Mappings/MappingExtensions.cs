using SpiderMem.Application.DTOs;
using SpiderMem.Domain.Entities;

namespace SpiderMem.Application.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this User user) =>
        new(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, user.CreatedAt);

    public static TagDto ToDto(this Tag tag) =>
        new(tag.Id, tag.Name);

    public static CommentDto ToDto(this Comment comment) =>
        new(comment.Id, comment.Content, comment.CreatedAt, comment.UserId, comment.MemeId);

    public static LikeDto ToDto(this Like like) =>
        new(like.Id, like.UserId, like.CreatedAt);

    public static MemeDto ToDto(this Meme meme) =>
        new(
            meme.Id,
            meme.Title,
            meme.ImageUrl,
            meme.CreatedAt,
            meme.UserId,
            meme.User?.UserName ?? "Unknown",
            meme.Tags.Select(t => t.ToDto()).ToList(),
            meme.Comments.Select(c => c.ToDto()).ToList(),
            meme.Likes.Count
        );

    public static IEnumerable<MemeDto> ToDtos(this IEnumerable<Meme> memes) =>
        memes.Select(m => m.ToDto());
}
