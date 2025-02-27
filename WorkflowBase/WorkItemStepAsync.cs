using WorkflowCore.Models;

namespace WorkerDemo.WorkflowBase
{
    public abstract class WorkItemStepAsync : StepBodyAsync
    {
        public WorkflowConfig? WorkflowConfig { get; set; }

    }
}
