using Application.Common.Constants;
using Application.Common.Models;
using Application.Infra.Context;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1;

public sealed record GetCustomersQueryResponse(
    Guid Id,
    int AlternateId,
    string Description,
    string Document,
    string? Email,
    string? Phone,
    DateTime CreatedAt,
    DateTime? DeactivatedAt
);

public sealed record GetCustomersQuery : IRequest<ICollection<GetCustomersQueryResponse>>;

public sealed class GetCustomersQueryHandler(DatabaseContext dbContext)
    : IRequestHandler<GetCustomersQuery, ICollection<GetCustomersQueryResponse>>
{
    public async Task<ICollection<GetCustomersQueryResponse>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken
    )
    {
        var customers = await dbContext.Customers.ToListAsync(cancellationToken);

        return customers
            .OrderBy(customer => customer.DeactivatedUtc != null)
            .ThenByDescending(customer => customer.CreatedUtc)
            .Select(customer => new GetCustomersQueryResponse(
                customer.Id,
                customer.AlternateId,
                customer.Description,
                customer.Document,
                customer.Email,
                customer.Phone,
                customer.CreatedUtc,
                customer.DeactivatedUtc
            ))
            .ToList();
    }
}

public sealed class GetCustomersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                $"{EndpointConstants.V1}{EndpointConstants.Customer}",
                async ([FromServices] ISender sender) =>
                {
                    var result = await sender.Send(new GetCustomersQuery());
                    return Result.Ok(result);
                }
            )
            .RequireAuthorization()
            .WithTags(SwaggerTagsConstants.Customer);
    }
}
