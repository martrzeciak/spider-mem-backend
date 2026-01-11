using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderMem.API.Extensions;
using SpiderMem.Application.Common;

namespace SpiderMem.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("IMediator service is unavailable");

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsFailure)
            return HandleError(result.Error);

        return Ok(result.Value);
    }

    protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
    {
        if (result.IsFailure)
            return HandleError(result.Error);

        Response.AddPaginationHeader(result.Value.CurrentPage,
            result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);

        return Ok(result.Value);
    }

    private ActionResult HandleError(Error error)
    {
        return error.Code switch
        {
            "400" => BadRequest(error),
            "401" => Unauthorized(error),
            "404" => NotFound(error),
            _ => StatusCode(500, error)
        };
    }
}