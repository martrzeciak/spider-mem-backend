namespace SpiderMem.Application.DTOs;

public record UserDto(Guid Id, string UserName, string Email, DateTime CreatedAt);
