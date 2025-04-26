using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Infra.Context;
using Carter;
using Domain.Entities;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.V1;

public sealed record CreateCustomerCommand(
    string Description,
    string Document,
    string? Email,
    string? Phone
) : IRequest<bool>;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Document is required.")
            .Must(x => x.IsValidDocument())
            .WithMessage("Invalid document number.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .Must(x => x.IsValidPhone())
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Invalid phone number.");
    }
}

public sealed class CreateCustomerCommandHandler(DatabaseContext dbContext)
    : IRequestHandler<CreateCustomerCommand, bool>
{
    public async Task<bool> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken
    )
    {
        if (VerifyCustomerExists(request.Document, request.Email))
            throw new EntityAlreadyExistsException("Customer");

        var customer = new Customer.Builder()
            .WithDescription(request.Description)
            .WithDocument(request.Document)
            .WithEmail(request.Email)
            .WithPhone(request.Phone)
            .Build();

        await dbContext.Customers.AddAsync(customer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private bool VerifyCustomerExists(string document, string? email)
    {
        return dbContext.Customers.Any(x => x.Document == document || x.Email == email);
    }
}

public sealed class CreateCustomerCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"{EndpointConstants.V1}{EndpointConstants.Customer}",
                async ([FromServices] ISender sender, [FromBody] CreateCustomerCommand command) =>
                {
                    var result = await sender.Send(command);
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Customer)
            .RequireAuthorization();
    }
}
