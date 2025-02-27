using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow_Demo;

public class DemoWorkflow : IWorkflow
{
    public string Id => "Demo";

    public int Version => 1;


    public void Build(IWorkflowBuilder<object> builder)
    {
        builder
            .UseDefaultErrorBehavior(WorkflowErrorHandling.Suspend)
            .StartWith<StepA>()
            .Then<StepB>();
    }
}
