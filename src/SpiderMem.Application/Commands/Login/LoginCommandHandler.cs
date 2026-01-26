using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Common;
using SpiderMem.Persistence.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace SpiderMem.Application.Commands.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public LoginCommandHandler(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.LoginDto.Email, cancellationToken);
        if (user == null)
            return Result.Failure<AuthResponseDto>(new Error("Auth.UserNotFound", "Użytkownik nie istnieje."));

        var hashedInputPassword = HashPassword(request.LoginDto.Password);
        if (user.PasswordHash != hashedInputPassword)
            return Result.Failure<AuthResponseDto>(new Error("Auth.WrongPassword", "Niepoprawne hasło."));

        var token = GenerateJwtToken(user);

        var authDto = new AuthResponseDto(token, user.UserName, user.Email);
        return Result.Success(authDto);
    }

    private string GenerateJwtToken(Domain.Entities.User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
