using System.Diagnostics;
using WorkerDemo.Generic.WalzWorkflow;

namespace WorkerDemo.Workflow_Demo;

class StepA : WalzStepBodyAsync
{
    public override async Task RunAsync()
    {
        Debug.WriteLine("Hello");

        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(500);
            await LogAsync($"looping {i + 1}/10");
        }

        await Assets.WriteStringAsync("dummy.txt", () =>
        {
            return "change this line manually to HELLO";
        });

        //Random random = new Random();
        //if (random.Next(2) == 0)
        //    throw new Exception("simulierte Ausnahme");
    }
}