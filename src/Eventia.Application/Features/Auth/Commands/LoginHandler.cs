using Eventia.Application.Common;
using Eventia.Application.DTOs.Auth;
using Eventia.Application.Interfaces;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<BaseResponse<AuthResponse>>;

public class LoginHandler : IRequestHandler<LoginCommand, BaseResponse<AuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<BaseResponse<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !user.IsActive || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return BaseResponse<AuthResponse>.Failure("Invalid credentials or inactive user");
        }

        var token = _tokenService.GenerateToken(user);
        var response = new AuthResponse(token, user.Email, user.Name, user.Role.ToString());

        return BaseResponse<AuthResponse>.SuccessResult(response, "Login successful");
    }
}
