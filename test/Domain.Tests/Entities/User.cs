using Domain.Entities;
using Domain.Exceptions;
using Domain.Extensions;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Should_Create_User_When_Data_Is_Valid()
    {
        var user = new User.Builder()
            .WithName("John Doe")
            .WithEmail("john.doe@email.com")
            .WithDocument("12345678909")
            .WithPhone("11999999999")
            .WithPassword("securePassword123")
            .Build();

        Assert.Equal("John Doe", user.Name);
        Assert.Equal("john.doe@email.com", user.Email);
        Assert.Equal("12345678909", user.Document);
        Assert.Equal("11999999999", user.Phone);
        Assert.Equal("securePassword123".Encrypt(), user.Password);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Name_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new User.Builder()
                    .WithName("")
                    .WithEmail("john.doe@email.com")
                    .WithDocument("12345678909")
                    .WithPhone("11999999999")
                    .WithPassword("securePassword123")
                    .Build()
        );

        Assert.Equal("Name", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_InvalidEmailFormatException_When_Email_Is_Invalid()
    {
        var invalidEmail = "john.email.com";

        var exception = Assert.Throws<InvalidEmailFormatException>(
            () =>
                new User.Builder()
                    .WithName("John Doe")
                    .WithEmail(invalidEmail)
                    .WithDocument("12345678909")
                    .WithPhone("11999999999")
                    .WithPassword("securePassword123")
                    .Build()
        );

        Assert.Equal(invalidEmail, exception.Email);
    }

    [Fact]
    public void Should_Throw_InvalidDocumentNumberException_When_Document_Is_Invalid()
    {
        var invalidDocument = "0000000000";

        var exception = Assert.Throws<InvalidDocumentNumberException>(
            () =>
                new User.Builder()
                    .WithName("John Doe")
                    .WithEmail("john.doe@email.com")
                    .WithDocument(invalidDocument)
                    .WithPhone("11999999999")
                    .WithPassword("securePassword123")
                    .Build()
        );

        Assert.Equal(invalidDocument, exception.Document);
    }

    [Fact]
    public void Should_Throw_InvalidPhoneNumberException_When_Phone_Is_Invalid()
    {
        var invalidPhone = "123";

        var exception = Assert.Throws<InvalidPhoneNumberException>(
            () =>
                new User.Builder()
                    .WithName("John Doe")
                    .WithEmail("john.doe@email.com")
                    .WithDocument("12345678909")
                    .WithPhone(invalidPhone)
                    .WithPassword("securePassword123")
                    .Build()
        );

        Assert.Equal(invalidPhone, exception.Phone);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Password_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new User.Builder()
                    .WithName("John Doe")
                    .WithEmail("john.doe@email.com")
                    .WithDocument("12345678909")
                    .WithPhone("11999999999")
                    .WithPassword("")
                    .Build()
        );

        Assert.Equal("Password", exception.FieldName);
    }

    [Fact]
    public void Should_Allow_Null_Phone()
    {
        var user = new User.Builder()
            .WithName("John Doe")
            .WithEmail("john.doe@email.com")
            .WithDocument("12345678909")
            .WithPhone(null)
            .WithPassword("securePassword123")
            .Build();

        Assert.Null(user.Phone);
    }

    [Fact]
    public void Should_Match_Password_When_Correct_Password_Is_Provided()
    {
        var password = "securePassword123";
        var confirmPassword = "securePassword123";
        var user = new User.Builder()
            .WithName("John Doe")
            .WithEmail("john.doe@email.com")
            .WithDocument("12345678909")
            .WithPhone("11999999999")
            .WithPassword(password)
            .Build();

        var isMatch = user.MatchPassword(confirmPassword);

        Assert.True(isMatch);
    }

    [Fact]
    public void Should_Not_Match_Password_When_Incorrect_Password_Is_Provided()
    {
        var password = "securePassword123";
        var incorrectPassword = "wrongPassword456";

        var user = new User.Builder()
            .WithName("John Doe")
            .WithEmail("john.doe@email.com")
            .WithDocument("12345678909")
            .WithPhone("11999999999")
            .WithPassword(password)
            .Build();

        var isMatch = user.MatchPassword(incorrectPassword);

        Assert.False(isMatch);
    }
}
