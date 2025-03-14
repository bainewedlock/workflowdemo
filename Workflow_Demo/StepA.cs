using WorkerDemo.Generic.WalzWorkflow;

namespace WorkerDemo.Workflow_Demo;

class StepA : WalzStepBodyAsync
{
    public override async Task RunAsync()
    {
        var n = 5;
        for (int i = 0; i < n; i++)
        {
            await Task.Delay(500);
            await LogAsync($"looping {i + 1}/{n}");
        }

        await Assets.WriteStringAsync("dummy.txt", () =>
        {
            return "change this line manually to HELLO so step 2 can resume";
        });
    }
}