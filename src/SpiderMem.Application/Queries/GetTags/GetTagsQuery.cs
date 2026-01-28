using SpiderMem.Application.DTOs;
using MediatR;

namespace SpiderMem.Application.Queries.GetTags;

public class GetTagsQuery : IRequest<Result<List<TagDto>>>{}
