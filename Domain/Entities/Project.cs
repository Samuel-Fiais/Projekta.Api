using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;

namespace Domain.Entities;

public class Project : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public ProjectScope Scope { get; private set; }
    public ProjectStatus Status { get; private set; } = ProjectStatus.Planned;

    public ICollection<Activity> Activities { get; private set; } = [];
    public ICollection<TeamMember> TeamMembers { get; private set; } = [];

    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new RequiredFieldException(nameof(Name));

        if (string.IsNullOrWhiteSpace(Description))
            throw new RequiredFieldException(nameof(Description));

        if (StartDate == default)
            throw new RequiredFieldException(nameof(StartDate));

        if (EndDate != null && EndDate < StartDate)
            throw new InvalidDateRangeException(StartDate, EndDate.Value);

        if (Scope == ProjectScope.Closed && EndDate == null)
            throw new RequiredFieldException(nameof(EndDate));

        if (!Enum.IsDefined(Scope))
            throw new InvalidEnumValueException(nameof(Scope), Scope.GetValue());

        if (!Enum.IsDefined(Status))
            throw new InvalidEnumValueException(nameof(Status), Status.GetValue());

        if (CustomerId == default)
            throw new RequiredFieldException(nameof(CustomerId));
    }

    public class Builder
    {
        private readonly Project _project = new();

        public Builder WithName(string name)
        {
            _project.Name = name;
            return this;
        }

        public Builder WithDescription(string description)
        {
            _project.Description = description;
            return this;
        }

        public Builder WithStartDate(DateTime startDate)
        {
            _project.StartDate = startDate;
            return this;
        }

        public Builder WithEndDate(DateTime? endDate)
        {
            _project.EndDate = endDate;
            return this;
        }

        public Builder WithScope(ProjectScope scope)
        {
            _project.Scope = scope;
            return this;
        }

        public Builder WithStatus(ProjectStatus status)
        {
            _project.Status = status;
            return this;
        }

        public Builder WithCustomerId(Guid customerId)
        {
            _project.CustomerId = customerId;
            return this;
        }

        public Project Build()
        {
            _project.Validate();
            return _project;
        }
    }
}
