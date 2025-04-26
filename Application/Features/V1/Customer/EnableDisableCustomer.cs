using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Infra.Context;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.V1;

public sealed record EnableDisableCustomerCommand(Guid Id) : IRequest<bool>;

public sealed class EnableDisableCustomerCommandValidator
    : AbstractValidator<EnableDisableCustomerCommand>
{
    public EnableDisableCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public sealed class EnableDisableCustomerCommandHandler(DatabaseContext dbContext)
    : IRequestHandler<EnableDisableCustomerCommand, bool>
{
    public async Task<bool> Handle(
        EnableDisableCustomerCommand request,
        CancellationToken cancellationToken
    )
    {
        var customer =
            await dbContext.Customers.FindAsync(request.Id)
            ?? throw new NotFoundException("Customer");

        customer.EnableDisable();

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public sealed class EnableDisableCustomerCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                $"{EndpointConstants.V1}{EndpointConstants.Customer}/{{id:guid}}/enable-disable",
                async ([FromServices] ISender sender, [FromRoute] Guid id) =>
                {
                    var command = new EnableDisableCustomerCommand(id);
                    var result = await sender.Send(command);
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Customer)
            .RequireAuthorization();
    }
}
