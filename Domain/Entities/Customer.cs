using Domain.Exceptions;
using Domain.Extensions;

namespace Domain.Entities;

public sealed class Customer : EntityBase
{
    public string Description { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public string? Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; } = string.Empty;

    public ICollection<Project> Projects { get; private set; } = [];

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new RequiredFieldException(nameof(Description));

        if (!Document.IsValidDocument())
            throw new InvalidDocumentNumberException(Document);

        if (Email != null && !Email.IsValidEmail())
            throw new InvalidEmailFormatException(Email);

        if (Phone != null && !Phone.IsValidPhone())
            throw new InvalidPhoneNumberException(Phone);
    }

    public class Builder
    {
        private readonly Customer _customer = new();

        public Builder WithDescription(string description)
        {
            _customer.Description = description;
            return this;
        }

        public Builder WithDocument(string document)
        {
            _customer.Document = document;
            return this;
        }

        public Builder WithEmail(string? email)
        {
            _customer.Email = email;
            return this;
        }

        public Builder WithPhone(string? phone)
        {
            _customer.Phone = phone;
            return this;
        }

        public Customer Build()
        {
            _customer.Validate();
            return _customer;
        }
    }
}
