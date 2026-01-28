using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Queries.GetUserDetails;

public record GetUserDetailsQuery : IRequest<Result<UserDetailsDto>>
{
    public Guid Id { get; set; }
};