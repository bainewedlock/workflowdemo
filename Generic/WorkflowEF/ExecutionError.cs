namespace WorkerDemo.Generic.WorkflowEF;

public partial class ExecutionError
{
    public int PersistenceId { get; set; }

    public string? WorkflowId { get; set; }

    public string? ExecutionPointerId { get; set; }

    public string ErrorTime { get; set; } = null!;

    public string? Message { get; set; }
}
