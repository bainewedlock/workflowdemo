using System.ComponentModel.DataAnnotations;

namespace WorkerDemo.Generic.WorkflowEF;

public partial class Workflow
{
    public int PersistenceId { get; set; }

    [Display(Name = "Instance")]
    public Guid InstanceId { get; set; }

    [Display(Name = "Workflow")]
    public string? WorkflowDefinitionId { get; set; }

    public int Version { get; set; }

    public string? Description { get; set; }

    public string? Reference { get; set; }

    public long? NextExecution { get; set; }

    public string? Data { get; set; }

    [Display(Name = "Start")]
    public DateTime CreateTime { get; set; }

    [Display(Name = "End")]
    public DateTime? CompleteTime { get; set; }

    public int Status { get; set; }

    public virtual ICollection<ExecutionPointer> ExecutionPointers { get; set; } = new List<ExecutionPointer>();
}
