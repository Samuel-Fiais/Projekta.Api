using Domain.Exceptions;

namespace Domain.Entities;

public class TeamMember : EntityBase
{
    public decimal HourlyRate { get; private set; }

    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }

    public override void Validate()
    {
        if (HourlyRate <= 0)
            throw new InvalidHourlyRateException(HourlyRate);

        if (UserId == default)
            throw new RequiredFieldException(nameof(UserId));

        if (ProjectId == default)
            throw new RequiredFieldException(nameof(ProjectId));
    }

    public class Builder
    {
        private readonly TeamMember _teamMember = new();

        public Builder WithHourlyRate(decimal hourlyRate)
        {
            _teamMember.HourlyRate = hourlyRate;
            return this;
        }

        public Builder WithUserId(Guid userId)
        {
            _teamMember.UserId = userId;
            return this;
        }

        public Builder WithProjectId(Guid projectId)
        {
            _teamMember.ProjectId = projectId;
            return this;
        }

        public TeamMember Build()
        {
            _teamMember.Validate();
            return _teamMember;
        }
    }
}
