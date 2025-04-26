using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Tests.Entities;

public class ProjectTests
{
    [Fact]
    public void Should_Create_Project_When_Data_Is_Valid()
    {
        var project = new Project.Builder()
            .WithName("Migration Project")
            .WithDescription("Cloud migration for client")
            .WithStartDate(DateTime.Today)
            .WithEndDate(DateTime.Today.AddDays(30))
            .WithScope(ProjectScope.Open)
            .WithStatus(ProjectStatus.Planned)
            .WithCustomerId(Guid.NewGuid())
            .Build();

        Assert.Equal("Migration Project", project.Name);
        Assert.Equal(ProjectScope.Open, project.Scope);
        Assert.Equal(ProjectStatus.Planned, project.Status);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Name_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Project.Builder()
                    .WithName("")
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithScope(ProjectScope.Open)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("Name", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Description_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("")
                    .WithStartDate(DateTime.Today)
                    .WithScope(ProjectScope.Open)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("Description", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_StartDate_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("Test")
                    .WithStartDate(default)
                    .WithScope(ProjectScope.Open)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("StartDate", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_InvalidDateRangeException_When_EndDate_Is_Before_StartDate()
    {
        var startDate = DateTime.Today;
        var endDate = startDate.AddDays(-1);

        var exception = Assert.Throws<InvalidDateRangeException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("Test")
                    .WithStartDate(startDate)
                    .WithEndDate(endDate)
                    .WithScope(ProjectScope.Open)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal(startDate, exception.StartDate);
        Assert.Equal(endDate, exception.EndDate);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Scope_Is_Closed_And_EndDate_Is_Null()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithScope(ProjectScope.Closed)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("EndDate", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_InvalidEnumValueException_When_Scope_Is_Invalid()
    {
        var invalidScope = (ProjectScope)99;

        var exception = Assert.Throws<InvalidEnumValueException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithScope(invalidScope)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("Scope", exception.EnumName);
        Assert.Equal("99", exception.Value);
    }

    [Fact]
    public void Should_Throw_InvalidEnumValueException_When_Status_Is_Invalid()
    {
        var invalidStatus = (ProjectStatus)77;

        var exception = Assert.Throws<InvalidEnumValueException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithScope(ProjectScope.Open)
                    .WithStatus(invalidStatus)
                    .WithCustomerId(Guid.NewGuid())
                    .Build()
        );

        Assert.Equal("Status", exception.EnumName);
        Assert.Equal("77", exception.Value);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_CustomerId_Is_Default()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                new Project.Builder()
                    .WithName("Test")
                    .WithDescription("Test")
                    .WithStartDate(DateTime.Today)
                    .WithScope(ProjectScope.Open)
                    .WithCustomerId(Guid.Empty)
                    .Build()
        );

        Assert.Equal("CustomerId", exception.FieldName);
    }

    [Fact]
    public void Should_Allow_Null_EndDate_When_Scope_Is_Open()
    {
        var project = new Project.Builder()
            .WithName("Test")
            .WithDescription("Test")
            .WithStartDate(DateTime.Today)
            .WithScope(ProjectScope.Open)
            .WithCustomerId(Guid.NewGuid())
            .Build();

        Assert.Null(project.EndDate);
    }
}
