using MediatR;
using SpiderMem.Application.DTOs;

namespace SpiderMem.Application.Commands.CreateComment;

public record CreateCommentCommand(CommentDto CommentDto) : IRequest<Result<CommentDto>>
{
        public Guid MemeId { get; set; }
        public string Content { get; set; } = null!;
    
}