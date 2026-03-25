using Eventia.Application.Features.Users.Commands;
using Eventia.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventia.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPatch("{id}/disable")]
    [Authorize(Roles = "Administrator")] // Only admin can disable users
    public async Task<IActionResult> Disable(Guid id)
    {
        var result = await _mediator.Send(new DisableUserCommand(id));
        return result.Success ? Ok(result) : NotFound(result);
    }
}
