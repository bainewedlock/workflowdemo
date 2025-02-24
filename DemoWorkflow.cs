using System.Diagnostics;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public class DemoWorkflow : IWorkflow
{
    public string Id => "Demo";

    public int Version => 1;

    public void Build(IWorkflowBuilder<object> builder)
    {
        builder
            .StartWith(ctx =>
            {
                Debug.WriteLine("Hello");
                return ExecutionResult.Next();

            })
            .Then(ctx =>
            {
                Debug.WriteLine("Good bye");
                return ExecutionResult.Next();
            });
    }
}