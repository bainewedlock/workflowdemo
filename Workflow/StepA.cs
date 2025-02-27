using System.Diagnostics;
using WorkerDemo.SignalR;
using WorkerDemo.WorkflowBase;
using WorkflowCore.Interface;
using WorkflowCore.Models;

class StepA : WorkItemStepAsync
{
    public override async Task<ExecutionResult> RunAsync(
        IStepExecutionContext ctx)
    {
        Debug.WriteLine("Hello");

        for(int i=0; i< 10; i++)
        {
            await Task.Delay(2000);
            ctx.LogSignal($"looping {i+1}/10");
        }

        //Random random = new Random();
        //if (random.Next(2) == 0)
        //    throw new Exception("simulierte Ausnahme");

        return ExecutionResult.Next();
    }
}