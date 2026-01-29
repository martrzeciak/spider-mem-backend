using Microsoft.AspNetCore.Http;

namespace SpiderMem.Application.DTOs;

public record AddMemeDto(
    string Title,
    List<Guid>? Tags1,
    IFormFile ImageUrl = default!
);