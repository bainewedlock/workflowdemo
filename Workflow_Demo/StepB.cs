namespace Workflow_Demo;

public class StepB : WorkItemStepAsync
{
    public override async Task RunAsync()
    {
        var text = (await ReadStringAssetAsync("dummy.txt")).Trim();
        if (text == "HELLO")
        {
            Log($"got correct text from asset: {text}");
        }
        else
        {
            Log($"got unexpected text from asset: {text}");
            throw new Exception("simulated exception");
        }
    }
}
