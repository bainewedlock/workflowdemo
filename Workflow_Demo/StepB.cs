namespace Workflow_Demo;

public class StepB : WorkItemStepAsync
{
    public override async Task RunAsync()
    {
        var text = ReadStringAsset("dummy.txt");
        Log($"got from asset: {text}");
    }
}
