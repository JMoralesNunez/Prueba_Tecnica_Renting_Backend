using Eventia.Application.Common;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Tickets.Commands;

public record DeleteTicketCommand(Guid Id) : IRequest<BaseResponse<bool>>;

public class DeleteTicketHandler : IRequestHandler<DeleteTicketCommand, BaseResponse<bool>>
{
    private readonly ITicketRepository _ticketRepository;

    public DeleteTicketHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.Id);
        if (ticket == null)
        {
            return BaseResponse<bool>.Failure("Ticket not found");
        }

        await _ticketRepository.DeleteAsync(ticket);
        return BaseResponse<bool>.SuccessResult(true, "Ticket deleted successfully");
    }
}
