using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Generic.WalzWorkflow;

public abstract class WalzStepBodyAsync : StepBodyAsync
{
    public WalzWorkflowConfig WorkflowConfig { get; set; } = null!;
    public ClientManager ClientManager { get; set; } = null!;
    public Assets.Factory AssetsFactory { get; set; } = null!;
    protected Assets Assets { get; set; } = null!;
    WorkflowStep Step { get; set; } = null!;

    public sealed override async Task<ExecutionResult> RunAsync(
        IStepExecutionContext stepctx)
    {
        Assets = AssetsFactory(stepctx.Workflow.Id);
        Step = stepctx.Step;

        await RunAsync();

        var m = $"End of step '{stepctx.Step.Name}'.";
        await Assets.LogAsync(LogCategory.Workflow, m);

        return ExecutionResult.Next();
    }

    public abstract Task RunAsync();

    /// <summary>
    /// Log Step message
    /// </summary>
    /// <param name="message">any line of text</param>
    protected Task LogAsync(string message)
    {
        return Assets.LogAsync(Step, message);
    }

}
