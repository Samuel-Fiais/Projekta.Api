using Application.Common.Constants;
using Application.Common.Models;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.V1;

public sealed class HealthCheckQuery : IRequest<DateTime>;

public sealed class HealthCheckQueryHandler : IRequestHandler<HealthCheckQuery, DateTime>
{
    public Task<DateTime> Handle(HealthCheckQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(DateTime.UtcNow);
    }
}

public sealed class HealthCheckEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                $"{EndpointConstants.V1}{EndpointConstants.HealthCheck}",
                async (ISender sender) =>
                {
                    var result = await sender.Send(new HealthCheckQuery());
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.HealthCheck);
    }
}
