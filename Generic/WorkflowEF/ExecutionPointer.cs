namespace WorkerDemo.Generic.WorkflowEF;

public partial class ExecutionPointer
{
    public int PersistenceId { get; set; }

    public int WorkflowId { get; set; }

    public string? Id { get; set; }

    public int StepId { get; set; }

    public int Active { get; set; }

    public string? SleepUntil { get; set; }

    public string? PersistenceData { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? EventName { get; set; }

    public string? EventKey { get; set; }

    public int EventPublished { get; set; }

    public string? EventData { get; set; }

    public string? StepName { get; set; }

    public int RetryCount { get; set; }

    public string? Children { get; set; }

    public string? ContextItem { get; set; }

    public string? PredecessorId { get; set; }

    public string? Outcome { get; set; }

    public int Status { get; set; }

    public string? Scope { get; set; }

    public virtual ICollection<ExtensionAttribute> ExtensionAttributes { get; set; } = new List<ExtensionAttribute>();

    public virtual Workflow Workflow { get; set; } = null!;
}
