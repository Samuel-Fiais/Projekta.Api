using System.ComponentModel;

namespace Domain.Enums;

public enum ProjectStatus
{
    [Description("Planned")]
    Planned = 1,

    [Description("In Progress")]
    InProgress = 2,

    [Description("On Hold")]
    Paused = 3,

    [Description("Completed")]
    Completed = 4,

    [Description("Cancelled")]
    Cancelled = 5,
}
