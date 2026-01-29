using MediatR;
using Microsoft.EntityFrameworkCore;
using SpiderMem.Application.DTOs;
using SpiderMem.Application.Mappings;
using SpiderMem.Application.Common;
using SpiderMem.Persistence.Data;
using SpiderMem.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SpiderMem.Application.Commands.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDto>>
    {
        private readonly AppDbContext _context;

        public RegisterUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                return Result.Failure<UserDto>(new Error("Auth.UserNameIsEmpty", "Podaj nazwę użytkownika."));
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result.Failure<UserDto>(new Error("Auth.EmailIsEmpty", "Podaj adres email."));
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return Result.Failure<UserDto>(new Error("Auth.PasswordIsEmpty", "Podaj hasło."));
            }

            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName || u.Email == request.Email, cancellationToken))
            {
                return Result.Failure<UserDto>(Error.AlreadyUsed("User"));
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = HashPassword(request.Password) // hashowanie hasła
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = user.ToDto();
            return Result<UserDto>.Success(dto);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        
    }
}
