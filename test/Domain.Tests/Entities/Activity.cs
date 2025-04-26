using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities;

public class ActivityTests
{
    [Fact]
    public void Should_Create_Activity_When_Data_Is_Valid()
    {
        var activity = new Activity.Builder()
            .WithDescription("Planning meeting")
            .WithStartDate(DateTime.Today)
            .WithEndDate(DateTime.Today.AddDays(1))
            .WithProjectId(Guid.NewGuid())
            .WithUserId(Guid.NewGuid())
            .Build();

        Assert.Equal("Planning meeting", activity.Description);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Description_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Activity.Builder()
                    .WithDescription("")
                    .WithStartDate(DateTime.Today)
                    .WithProjectId(Guid.NewGuid())
                    .WithUserId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("Description", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_StartDate_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Activity.Builder()
                    .WithDescription("Test")
                    .WithStartDate(default)
                    .WithProjectId(Guid.NewGuid())
                    .WithUserId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("StartDate", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_InvalidDateRangeException_When_EndDate_Is_Before_StartDate()
    {
        var start = DateTime.Today;
        var end = start.AddDays(-1);

        var exception = Assert.Throws<InvalidDateRangeException>(
            () =>
                new Activity.Builder()
                    .WithDescription("Test")
                    .WithStartDate(start)
                    .WithEndDate(end)
                    .WithProjectId(Guid.NewGuid())
                    .WithUserId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal(start, exception.StartDate);
        Assert.Equal(end, exception.EndDate);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_ProjectId_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Activity.Builder()
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithProjectId(Guid.Empty)
                    .WithUserId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("ProjectId", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_UserId_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Activity.Builder()
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithProjectId(Guid.NewGuid())
                    .WithUserId(Guid.Empty)
                    .Build()
        );

        Assert.Equal("UserId", exception.FieldName);
    }

    [Fact]
    public void Should_Allow_Null_EndDate()
    {
        var activity = new Activity.Builder()
            .WithDescription("Planning meeting")
            .WithStartDate(DateTime.Today)
            .WithEndDate(null)
            .WithProjectId(Guid.NewGuid())
            .WithUserId(Guid.NewGuid())
            .Build();

        Assert.Null(activity.EndDate);
    }
}
