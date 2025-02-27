using System.Diagnostics;
using WorkerDemo.SignalR;
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

                for(int i=0; i< 10; i++)
                {
                    Thread.Sleep(3000);
                    SignalrService.Enqueue(new WorkflowMessage(
                        workflow: ctx.Workflow.Id, message: "Hello"));
                }

                //Random random = new Random();
                //if (random.Next(2) == 0)
                //    throw new Exception("simulierte Ausnahme");

                return ExecutionResult.Next();
            })
            .Then(ctx =>
            {
                Debug.WriteLine("Good bye");
                return ExecutionResult.Next();
            });
    }
}