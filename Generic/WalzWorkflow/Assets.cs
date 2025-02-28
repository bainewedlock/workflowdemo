using WorkflowCore.Models;

namespace WorkerDemo.Generic.WalzWorkflow;

public class Assets
{
    public delegate Assets Factory(string workflow_instance_id);
    readonly WalzWorkflowConfig config;
    readonly string wf_instance_id;
    readonly ClientManager client_manager;

    public WorkflowStep Step { get; internal set; } = null!;

    public Assets(WalzWorkflowConfig config, string workflow_instance_id,
        ClientManager client_manager)
    {
        this.config = config;
        wf_instance_id = workflow_instance_id;
        this.client_manager = client_manager;
    }

    /// <summary>
    /// Create asset dir for workflow if necessary
    /// </summary>
    /// <returns>path</returns>
    public string InitDir()
    {
        var path = Path.Combine(config.AssetsBaseDir, wf_instance_id);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    /// <summary>
    /// Write to current workflow log
    /// </summary>
    /// <param name="category">step|asset</param>
    /// <param name="message">any line of text</param>
    internal async Task LogAsync(LogCategory category, string message)
    {
        var timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        var category_s = category.ToString();
        var step_s = Step?.Name ?? "";

        var m = new WorkflowMessage(
                    workflow_id: wf_instance_id,
                    key: "Log",
                    data: new Dictionary<string, object>
                {
            { "timestamp", timestamp },
            { "category", category_s },
            { "step", step_s },
            { "message", message }
                });

        var line = string.Join(" ",
            timestamp,
            $"[{category_s}|{step_s}]",
            message);

        await AppendLogAsync(line);
        await client_manager.PublishAsync(m);
    }

    /// <summary>
    /// WriteText plus ReadText
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <param name="create">Callback to create asset</param>
    /// <returns>File content as string</returns>
    public async Task<string> StringAsync(
        string filename, Func<string> create)
    {
        await WriteStringAsync(filename, create);
        return await ReadStringAsync(filename);
    }

    /// <summary>
    /// Read content of existing utf-8 asset file
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <returns>File content as string</returns>
    public async Task<string> ReadStringAsync(string filename)
    {
        var path = Path.Combine(InitDir(), filename);
        return await File.ReadAllTextAsync(path);
    }

    /// <summary>
    /// Write utf-8 asset file
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <param name="create">Callback to create asset</param>
    public async Task WriteStringAsync(string filename, Func<string> create)
    {
        var path = Path.Combine(InitDir(), filename);
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
    /// Append one line to log
    /// </summary>
    /// <param name="message">preformatted line</param>
    /// <returns></returns>
    Task AppendLogAsync(string line)
    {
        var path = Path.Combine(InitDir(), "logfile.txt");
        return File.AppendAllLinesAsync(path, [line]);
    }
}
