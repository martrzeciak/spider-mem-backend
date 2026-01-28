using Microsoft.AspNetCore.Http;

namespace SpiderMem.Application.DTOs;

public record AddMemeDto(
    string Title,
    IFormFile ImageUrl,
    List<Guid>? Tags1
);