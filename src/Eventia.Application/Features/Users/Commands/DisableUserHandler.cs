using Eventia.Application.Common;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Users.Commands;

public record DisableUserCommand(Guid Id) : IRequest<BaseResponse<bool>>;

public class DisableUserHandler : IRequestHandler<DisableUserCommand, BaseResponse<bool>>
{
    private readonly IUserRepository _userRepository;

    public DisableUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<bool>> Handle(DisableUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            return BaseResponse<bool>.Failure("User not found");
        }

        user.IsActive = false;
        await _userRepository.UpdateAsync(user);

        return BaseResponse<bool>.SuccessResult(true, "User disabled successfully");
    }
}
