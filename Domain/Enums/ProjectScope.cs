using System.ComponentModel;

namespace Domain.Enums;

public enum ProjectScope
{
    [Description("Open")]
    Open = 1,

    [Description("Closed")]
    Closed = 2,
}
