using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities;

public class TeamMemberTests
{
    [Fact]
    public void Should_Create_TeamMember_When_Data_Is_Valid()
    {
        var teamMember = new TeamMember.Builder()
            .WithHourlyRate(120.50m)
            .WithUserId(Guid.NewGuid())
            .WithProjectId(Guid.NewGuid())
            .Build();

        Assert.Equal(120.50m, teamMember.HourlyRate);
    }

    [Fact]
    public void Should_Throw_InvalidHourlyRateException_When_HourlyRate_Is_Zero()
    {
        var exception = Assert.Throws<InvalidHourlyRateException>(
            () =>
                new TeamMember.Builder()
                    .WithHourlyRate(0)
                    .WithUserId(Guid.NewGuid())
                    .WithProjectId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal(0, exception.HourlyRate);
    }

    [Fact]
    public void Should_Throw_InvalidHourlyRateException_When_HourlyRate_Is_Negative()
    {
        var exception = Assert.Throws<InvalidHourlyRateException>(
            () =>
                new TeamMember.Builder()
                    .WithHourlyRate(-10)
                    .WithUserId(Guid.NewGuid())
                    .WithProjectId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal(-10, exception.HourlyRate);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_UserId_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new TeamMember.Builder()
                    .WithHourlyRate(100)
                    .WithUserId(Guid.Empty)
                    .WithProjectId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("UserId", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_ProjectId_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new TeamMember.Builder()
                    .WithHourlyRate(100)
                    .WithUserId(Guid.NewGuid())
                    .WithProjectId(Guid.Empty)
                    .Build()
        );

        Assert.Equal("ProjectId", exception.FieldName);
    }
}
