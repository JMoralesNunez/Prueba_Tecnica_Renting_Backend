using Eventia.Application.Common;
using Eventia.Application.Features.Tickets.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eventia.API.Controllers;

public partial class TicketsController
{
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] TicketStatusRequest request)
    {
        var result = await _mediator.Send(new ChangeTicketStatusCommand(id, request.Status));
        return result.Success ? Ok(result) : NotFound(result);
    }
}

public record TicketStatusRequest(Eventia.Domain.Enums.TicketStatus Status);
