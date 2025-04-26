using Application.Common.Exceptions;
using Application.Common.Interfaces.Helpers;
using Application.Features.V1;
using Application.Infra.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.Features.V1.Auth;

public class AuthenticateCommandHandlerTests
{
    private readonly DbContextOptions<DatabaseContext> _dbOptions;

    public AuthenticateCommandHandlerTests()
    {
        _dbOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    private static User CreateTestUser(string email, string password)
    {
        return new User.Builder()
            .WithName("Test User")
            .WithEmail(email)
            .WithDocument("12345678909")
            .WithPhone("11999999999")
            .WithPassword(password)
            .Build();
    }

    [Fact]
    public async Task Should_Return_Token_When_Credentials_Are_Valid()
    {
        var email = "john.doe@example.com";
        var password = "Password123";
        var user = CreateTestUser(email, password);

        var jwtServiceMock = new Mock<IJwtService>();
        jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("fake-jwt-token");

        using var context = new DatabaseContext(_dbOptions);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var command = new AuthenticateCommand(email, password);
        var handler = new AuthenticateCommandHandler(context, jwtServiceMock.Object);

        var result = await handler.Handle(command, default);

        Assert.Equal("fake-jwt-token", result);
    }

    [Fact]
    public async Task Should_Throw_PasswordOrEmailIncorrectException_When_User_Not_Found()
    {
        using var context = new DatabaseContext(_dbOptions);
        var jwtServiceMock = new Mock<IJwtService>();

        var command = new AuthenticateCommand("notfound@example.com", "anyPassword");
        var handler = new AuthenticateCommandHandler(context, jwtServiceMock.Object);

        await Assert.ThrowsAsync<PasswordOrEmailIncorrectException>(
            () => handler.Handle(command, default)
        );
    }

    [Fact]
    public async Task Should_Throw_PasswordOrEmailIncorrectException_When_Password_Is_Incorrect()
    {
        var email = "jane.doe@example.com";
        var user = CreateTestUser(email, "CorrectPassword");

        using var context = new DatabaseContext(_dbOptions);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var jwtServiceMock = new Mock<IJwtService>();
        var command = new AuthenticateCommand(email, "WrongPassword");
        var handler = new AuthenticateCommandHandler(context, jwtServiceMock.Object);

        await Assert.ThrowsAsync<PasswordOrEmailIncorrectException>(
            () => handler.Handle(command, default)
        );
    }
}
