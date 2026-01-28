namespace SpiderMem.Application.DTOs;

public record CommentDto(Guid Id, string Content, DateTime CreatedAt, Guid UserId, Guid MemeId, string UserName);