using System.Reflection;
using WorkerDemo.Generic.Workflows;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public abstract class WorkItemStepAsync : StepBodyAsync
{
    public WorkflowConfig WorkflowConfig { get; set; } = null!;
    public ClientManager ClientManager { get; set; } = null!;
    IStepExecutionContext stepctx { get; set; } = null!;

    public sealed override async Task<ExecutionResult> RunAsync(
        IStepExecutionContext stepctx)
    {
        this.stepctx = stepctx;
        await RunAsync();
        return ExecutionResult.Next();
    }

    public abstract Task RunAsync();

    /// <summary>
    /// Log Step message
    /// </summary>
    /// <param name="message">any line of text</param>
    protected Task LogAsync(string message)
    {
        return LogAsync(LogCategory.Step, message);
    }

    /// <summary>
    /// Write to current workflows log
    /// </summary>
    /// <param name="category">step|asset</param>
    /// <param name="message">any line of text</param>
    Task LogAsync(LogCategory category, string message)
    {
        var m = new WorkflowMessage(
                    workflow_id: stepctx.Workflow.Id,
                    key: "Log",
                    data: new Dictionary<string, object>
                {
                { "timestamp", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") },
                { "category", category.ToString() },
                { "step", stepctx.Step.Name },
                { "message", message }
                });

        return ClientManager.PublishAsync(m);
    }

    /// <summary>
    /// WriteTextAsset plus ReadTextAsset
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <param name="create">Callback to create asset</param>
    /// <returns>File content as string</returns>
    protected async Task<string> StringAssetAsync(
        string filename, Func<string> create)
    {
        await WriteStringAssetAsync(filename, create);
        return await ReadStringAssetAsync(filename);
    }

    /// <summary>
    /// Read content of existing utf-8 asset file
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <returns>File content as string</returns>
    protected async Task<string> ReadStringAssetAsync(string filename)
    {
        var dir = InitAssetDir();
        var path = Path.Combine(dir, filename);
        return await File.ReadAllTextAsync(path);
    }

    /// <summary>
    /// Write utf-8 asset file
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <param name="create">Callback to create asset</param>
    protected async Task WriteStringAssetAsync(string filename,
        Func<string> create)
    {
        var dir = InitAssetDir();
        var path = Path.Combine(dir, filename);
        if (Path.Exists(path))
        {
            await LogAsync(LogCategory.Asset,
                $"string asset already exists: {filename}");
        }
        else
        {
            var content = create(); // TODO: make create() async
            await File.WriteAllTextAsync(path, content);
            await LogAsync(LogCategory.Asset,
                $"string asset written: {filename} ({content.Length})");
        }
    }

    /// <summary>
    /// Create asset dir for workflow if necessary
    /// </summary>
    /// <returns>path</returns>
    string InitAssetDir()
    {
        var path = Path.Combine(
            WorkflowConfig.AssetsBaseDir, stepctx.Workflow.Id);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }
}
