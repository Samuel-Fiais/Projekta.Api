using Domain.Exceptions;

namespace Domain.Entities;

public class Activity : EntityBase
{
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }

    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new RequiredFieldException(nameof(Description));

        if (StartDate == default)
            throw new RequiredFieldException(nameof(StartDate));

        if (EndDate != null && EndDate < StartDate)
            throw new InvalidDateRangeException(StartDate, EndDate.Value);

        if (ProjectId == default)
            throw new RequiredFieldException(nameof(ProjectId));

        if (UserId == default)
            throw new RequiredFieldException(nameof(UserId));
    }

    public class Builder
    {
        private readonly Activity _activity = new();

        public Builder WithDescription(string description)
        {
            _activity.Description = description;
            return this;
        }

        public Builder WithStartDate(DateTime startDate)
        {
            _activity.StartDate = startDate;
            return this;
        }

        public Builder WithEndDate(DateTime? endDate)
        {
            _activity.EndDate = endDate;
            return this;
        }

        public Builder WithProjectId(Guid projectId)
        {
            _activity.ProjectId = projectId;
            return this;
        }

        public Builder WithUserId(Guid userId)
        {
            _activity.UserId = userId;
            return this;
        }

        public Activity Build()
        {
            _activity.Validate();
            return _activity;
        }
    }
}
