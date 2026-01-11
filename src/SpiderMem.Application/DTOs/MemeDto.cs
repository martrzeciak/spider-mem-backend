namespace SpiderMem.Application.DTOs;

public record MemeDto(
    Guid Id,
    string Title,
    string ImageUrl,
    DateTime CreatedAt,
    Guid UserId,
    string UserName,
    ICollection<TagDto> Tags,
    ICollection<CommentDto> Comments,
    int LikeCount);