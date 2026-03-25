using Eventia.Application.Common;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<BaseResponse<UserResponse>>;

public record UserResponse(Guid Id, string Name, string Email, string Role, bool IsActive);

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            return BaseResponse<UserResponse>.Failure("User not found");
        }

        var response = new UserResponse(user.Id, user.Name, user.Email, user.Role.ToString(), user.IsActive);
        return BaseResponse<UserResponse>.SuccessResult(response);
    }
}
