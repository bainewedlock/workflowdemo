using System;
using System.Collections.Generic;

namespace WorkerDemo.Model;

public partial class ScheduledCommand
{
    public int PersistenceId { get; set; }

    public string? CommandName { get; set; }

    public string? Data { get; set; }

    public int ExecuteTime { get; set; }
}
