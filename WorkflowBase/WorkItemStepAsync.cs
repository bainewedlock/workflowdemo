using WorkerDemo.SignalR;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public abstract class WorkItemStepAsync : StepBodyAsync
{
    public WorkflowConfig WorkflowConfig { get; set; } = null!;

    IStepExecutionContext Context { get; set; } = null!;

    public sealed override async Task<ExecutionResult> RunAsync(
        IStepExecutionContext context)
    {
        Context = context;
        await RunAsync();
        return ExecutionResult.Next();
    }

    public abstract Task RunAsync();

    protected void Log(string message)
    {
        Log("step", message);
    }

    void Log(string category, string message)
    {
        SignalrService.Enqueue(new WorkflowMessage(
            workflow_id: Context.Workflow.Id,
            key: "Log",
            data: new Dictionary<string, object>
        {
                { "timestamp", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") },
                { "category", category },
                { "step", Context.Step.Name },
                { "message", message }
        }));
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
            Log("asset", $"string asset already exists: {filename}");
        }
        else
        {
            var content = create(); // TODO: make create() async
            await File.WriteAllTextAsync(path, content);
            Log("asset",
                $"string asset written: {filename} ({content.Length})");
        }
    }

    string InitAssetDir()
    {
        var path = Path.Combine(
            WorkflowConfig.AssetsBaseDir, Context.Workflow.Id);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }
}
