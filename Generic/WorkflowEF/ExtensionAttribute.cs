namespace WorkerDemo.Generic.WorkflowEF;

public partial class ExtensionAttribute
{
    public int PersistenceId { get; set; }

    public int ExecutionPointerId { get; set; }

    public string? AttributeKey { get; set; }

    public string? AttributeValue { get; set; }

    public virtual ExecutionPointer ExecutionPointer { get; set; } = null!;
}
