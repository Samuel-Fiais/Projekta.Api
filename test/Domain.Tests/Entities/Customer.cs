using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities;

public class CustomerTests
{
    [Fact]
    public void Should_Create_Customer_When_Data_Is_Valid()
    {
        var customer = new Customer.Builder()
            .WithDescription("ACME Corporation")
            .WithDocument("16670073607")
            .WithEmail("contact@acme.com")
            .WithPhone("11999999999")
            .Build();

        Assert.Equal("ACME Corporation", customer.Description);
        Assert.Equal("16670073607", customer.Document);
        Assert.Equal("contact@acme.com", customer.Email);
        Assert.Equal("11999999999", customer.Phone);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Description_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Customer.Builder()
                    .WithDescription("")
                    .WithDocument("16670073607")
                    .WithEmail("contact@acme.com")
                    .WithPhone("11999999999")
                    .Build()
        );

        Assert.Equal("Description", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_InvalidDocumentNumberException_When_Document_Is_Invalid()
    {
        var invalidDocument = "00000000000";

        var exception = Assert.Throws<InvalidDocumentNumberException>(
            () =>
                new Customer.Builder()
                    .WithDescription("ACME Corporation")
                    .WithDocument(invalidDocument)
                    .WithEmail("contact@acme.com")
                    .WithPhone("11999999999")
                    .Build()
        );

        Assert.Equal(invalidDocument, exception.Document);
    }

    [Fact]
    public void Should_Throw_InvalidEmailFormatException_When_Email_Is_Invalid()
    {
        var invalidEmail = "acmeemail.com";

        var exception = Assert.Throws<InvalidEmailFormatException>(
            () =>
                new Customer.Builder()
                    .WithDescription("ACME Corporation")
                    .WithDocument("16670073607")
                    .WithEmail(invalidEmail)
                    .WithPhone("11999999999")
                    .Build()
        );

        Assert.Equal(invalidEmail, exception.Email);
    }

    [Fact]
    public void Should_Throw_InvalidPhoneNumberException_When_Phone_Is_Invalid()
    {
        var invalidPhone = "123";

        var exception = Assert.Throws<InvalidPhoneNumberException>(
            () =>
                new Customer.Builder()
                    .WithDescription("ACME Corporation")
                    .WithDocument("16670073607")
                    .WithEmail("contact@acme.com")
                    .WithPhone(invalidPhone)
                    .Build()
        );

        Assert.Equal(invalidPhone, exception.Phone);
    }

    [Fact]
    public void Should_Allow_Null_Email_And_Phone()
    {
        var customer = new Customer.Builder()
            .WithDescription("ACME Corporation")
            .WithDocument("16670073607")
            .WithEmail(null)
            .WithPhone(null)
            .Build();

        Assert.Null(customer.Email);
        Assert.Null(customer.Phone);
    }
}
