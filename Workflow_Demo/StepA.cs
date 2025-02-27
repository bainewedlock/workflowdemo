using System.Diagnostics;

namespace Workflow_Demo;

class StepA : WorkItemStepAsync
{
    public override async Task RunAsync()
    {
        Debug.WriteLine("Hello");

        for(int i=0; i< 5; i++)
        {
            await Task.Delay(500);
            Log($"looping {i+1}/10");
        }

        WriteStringAsset("dummy.txt", () =>
        {
            return "hallo welt";
        });

        //Random random = new Random();
        //if (random.Next(2) == 0)
        //    throw new Exception("simulierte Ausnahme");
    }
}