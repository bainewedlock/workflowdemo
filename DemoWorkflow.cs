using System.Diagnostics;
using WorkerDemo;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public class DemoWorkflow : IWorkflow
{
    public string Id => "Demo";

    public int Version => 1;


    public void Build(IWorkflowBuilder<object> builder)
    {
        builder
            .UseDefaultErrorBehavior(WorkflowErrorHandling.Suspend)
            .StartWith(ctx =>
            {
                Debug.WriteLine("Hello");

                Thread.Sleep(3000);

                SignalrService.Enqueue("foobarchen " + ctx.Workflow.Id);

                Random random = new Random();
                if (random.Next(2) == 0)
                    throw new Exception("simulierte Ausnahme");

                return ExecutionResult.Next();
            })
            .Then(ctx =>
            {
                Debug.WriteLine("Good bye");
                return ExecutionResult.Next();
            });
    }
}