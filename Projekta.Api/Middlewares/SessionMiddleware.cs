using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Projekta.Api.Middlewares;

public class SessionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.GetTypedHeaders().Headers.Authorization;

        var authorizeAttributes = context
            .GetEndpoint()
            ?.Metadata.GetOrderedMetadata<AuthorizeAttribute>();

        try
        {
            if (
                !string.IsNullOrEmpty(token)
                && authorizeAttributes != null
                && authorizeAttributes.Any()
            )
            {
                var jwt = context.Request.Headers.Authorization.ToString() ?? string.Empty;
                var jwtSecurityToken = new JwtSecurityToken(jwt.Split(" ")[1]);

                if (!string.IsNullOrEmpty(jwt))
                {
                    context.Session.Set("jwt_key", Encoding.UTF8.GetBytes(jwt.Split(" ")[1]));
                }

                if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
                {
                    throw new InvalidTokenException();
                }
            }
        }
        catch
        {
            throw new UnauthorizedException();
        }

        await next(context);
    }
}
