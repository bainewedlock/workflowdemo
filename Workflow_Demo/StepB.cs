using WorkerDemo.Generic.WalzWorkflow;

namespace WorkerDemo.Workflow_Demo;

public class StepB : WalzStepBodyAsync
{
    public override async Task RunAsync()
    {
        var text = (await Assets.ReadStringAsync("dummy.txt")).Trim();

        if (text == "HELLO")
        {
            await LogAsync($"got correct text from asset: {text}");
        }
        else
        {
            await LogAsync($"got unexpected text from asset: {text}");
            throw new Exception("simulated exception");
        }
    }
}
