using Domain.Entities;

namespace Application.Common.Interfaces.Helpers;

public interface IJwtService
{
    string GenerateToken(User user);
}
