using Eventia.Domain.Entities;

namespace Eventia.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
