namespace SpiderMem.Application.DTOsl;

public record CommentDto(Guid Id, string Content, DateTime CreatedAt, Guid UserId, string UserName);