using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Interfaces.Helpers;
using Microsoft.AspNetCore.Http;

namespace Application.Infra.Services.Helpers;

public class UserContext(IHttpContextAccessor context) : IUserContext
{
    public Guid? Id => GetClaim<Guid>("id");

    private T? GetClaim<T>(string name)
    {
        var claim = Jwt?.Claims?.FirstOrDefault(claim => claim.Type == name);
        if (claim is not null && typeof(T) == typeof(Guid))
        {
            return Guid.TryParse(claim.Value, out Guid output)
                ? (T)Convert.ChangeType(output, typeof(T))
                : default;
        }

        return claim is not null ? (T)Convert.ChangeType(claim.Value, typeof(T)) : default;
    }

    private JwtSecurityToken? Jwt
    {
        get
        {
            try
            {
                return new JwtSecurityTokenHandler().ReadJwtToken(GetRawJwt());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    private string? GetRawJwt()
    {
        var token = string.Empty;

        if (
            context.HttpContext != null
            && context.HttpContext.Session.TryGetValue("jwt_key", out var jwt)
        )
        {
            token = Encoding.UTF8.GetString(jwt);
        }

        return token;
    }
}
