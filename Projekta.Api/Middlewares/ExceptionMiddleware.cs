using System.Net;
using Application.Common.Exceptions;
using Application.Common.Models;
using Domain.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Projekta.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var r = Result.Failure(ex.Message);

            response.ContentType = "application/json";
            response.StatusCode = ex switch
            {
                DomainException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                InvalidTokenException => StatusCodes.Status401Unauthorized,
                PasswordOrEmailIncorrectException => StatusCodes.Status401Unauthorized,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                ApplicationExceptions => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            }

            await response.WriteAsync(
                JsonConvert.SerializeObject(
                    r,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    }
                )
            );
        }
    }
}
