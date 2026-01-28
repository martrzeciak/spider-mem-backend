namespace SpiderMem.Application.DTOs;

public record LoginDto(string Email, string Password);

public record AuthResponseDto(string Token, string UserName, string Email);
