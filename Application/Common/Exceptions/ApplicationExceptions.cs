using Application.Common.Constants;
using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ApplicationExceptions : Exception
{
    public ApplicationExceptions(string message)
        : base(message) { }

    public ApplicationExceptions(string message, Exception innerException)
        : base(message, innerException) { }
}

public class NotFoundException(string entityName)
    : ApplicationExceptions(string.Format(ErrorMessageConstants.NotFound, entityName)) { }

public class UnauthorizedException : ApplicationExceptions
{
    public UnauthorizedException()
        : base(ErrorMessageConstants.Unauthorized) { }
}

public class InvalidTokenException : ApplicationExceptions
{
    public InvalidTokenException()
        : base(ErrorMessageConstants.InvalidToken) { }
}

public class PasswordOrEmailIncorrectException : ApplicationExceptions
{
    public PasswordOrEmailIncorrectException()
        : base(ErrorMessageConstants.PasswordOrEmailIncorrect) { }
}

public class ValidationException(IEnumerable<ValidationFailure> failures)
    : ApplicationExceptions(string.Join(" / ", failures.Select(f => f.ErrorMessage)))
{
    public IEnumerable<ValidationFailure> Failures { get; } = failures;
}

public class EntityAlreadyExistsException(string entityName)
    : ApplicationExceptions(
        string.Format(ErrorMessageConstants.EntityAlreadyExists, entityName)
    ) { }
