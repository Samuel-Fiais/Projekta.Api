using Domain.Exceptions;
using Domain.Extensions;

namespace Domain.Entities;

public sealed class User : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public string? Phone { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;

    public ICollection<Activity> Activities { get; private set; } = [];
    public ICollection<TeamMember> TeamMembers { get; private set; } = [];

    public bool MatchPassword(string password)
    {
        return Password.Equals(password.Encrypt());
    }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new RequiredFieldException(nameof(Name));

        if (!Email.IsValidEmail())
            throw new InvalidEmailFormatException(Email);

        if (!Document.IsValidDocument())
            throw new InvalidDocumentNumberException(Document);

        if (Phone != null && !Phone.IsValidPhone())
            throw new InvalidPhoneNumberException(Phone);

        if (string.IsNullOrWhiteSpace(Password))
            throw new RequiredFieldException(nameof(Password));
    }

    public class Builder
    {
        private readonly User _user = new();

        public Builder WithName(string name)
        {
            _user.Name = name;
            return this;
        }

        public Builder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public Builder WithDocument(string document)
        {
            _user.Document = document;
            return this;
        }

        public Builder WithPhone(string? phone)
        {
            _user.Phone = phone;
            return this;
        }

        public Builder WithPassword(string password)
        {
            _user.Password = password;
            return this;
        }

        public User Build()
        {
            _user.Validate();
            _user.Password = _user.Password.Encrypt();
            return _user;
        }
    }
}
