using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Commands.Auth
{
    public class RegisterUserCommand : IRequest<Result<UserDto>>
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
