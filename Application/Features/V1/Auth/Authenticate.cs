using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Helpers;
using Application.Common.Models;
using Application.Infra.Context;
using Carter;
using Domain.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.V1;

public sealed record AuthenticateCommand(string Email, string Password) : IRequest<string>;

public sealed class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
{
    public AuthenticateCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}

public sealed class AuthenticateCommandHandler(DatabaseContext dbContext, IJwtService jwtService)
    : IRequestHandler<AuthenticateCommand, string>
{
    public async Task<string> Handle(
        AuthenticateCommand request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await dbContext.Users.FirstOrDefaultAsync(
                x => x.Email == request.Email,
                cancellationToken
            ) ?? throw new PasswordOrEmailIncorrectException();

        if (!user.MatchPassword(request.Password))
            throw new PasswordOrEmailIncorrectException();

        return jwtService.GenerateToken(user);
    }
}

public sealed class AuthenticateCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"{EndpointConstants.V1}{EndpointConstants.Auth}",
                async ([FromBody] AuthenticateCommand command, [FromServices] ISender sender) =>
                {
                    var result = await sender.Send(command);
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Auth);
    }
}
