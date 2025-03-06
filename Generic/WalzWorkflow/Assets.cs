using System.Text.RegularExpressions;
using WorkflowCore.Models;

namespace WorkerDemo.Generic.WalzWorkflow;

public class Assets
{
    public delegate Assets Factory(WorkflowInstance wf);
    readonly WalzWorkflowConfig config;
    readonly WorkflowInstance wf;
    readonly ClientManager client_manager;
    const string OWNER_FILE = "owner_workflow.txt";

    public Assets(WalzWorkflowConfig config, WorkflowInstance wf,
        ClientManager client_manager)
    {
        this.config = config;
        this.wf = wf;
        this.client_manager = client_manager;
    }

    /// <summary>
    /// Create asset dir for workflow if necessary
    /// </summary>
    /// <returns>path</returns>
    string InitDir()
    {
        var path = Path.Combine( config.AssetsBaseDir, SubDir);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    string SubDir => $"{wf.WorkflowDefinitionId}/{wf.Reference}";

    /// <summary>
    /// Write to current workflow log (from a workflow step)
    /// </summary>
    /// <param name="step"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task LogAsync(WorkflowStep step, string message)
    {
        return LogAsync($"Step:{step.Name}", message);
    }

    /// <summary>
    /// Write to current workflow log (with a custom category)
    /// </summary>
    /// <param name="category"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task LogAsync(LogCategory category, string message)
    {
        message = Regex.Replace(message, @"\r\n|\r|\n", "|");
        return LogAsync(category.ToString(), message);
    }

    async Task LogAsync(string context, string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        var log = new LogEntry(
                        time: timestamp,
                        message: message,
                        context: context);

        var m = new WalzWorkflowMessage(
                    workflow_id: wf.Id,
                    key: "Log",
                    data: new Dictionary<string, object>
                    {
                        { "add_log" , log }
                    }
                );

        await AppendLogAsync($"{timestamp} [{context}] {message}");
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
    /// Check is asset file exists
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <returns>File content as string</returns>
    public bool Exists(string filename)
    {
        var path = Path.Combine(InitDir(), filename);
        return File.Exists(path);
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

    string Logfile => $"{wf.Id}.log";

    /// <summary>
    /// Read content of existing utf-8 asset file
    /// </summary>
    /// <param name="filename">Filename without path</param>
    /// <returns>File content as string</returns>
    public async Task<string[]> ReadLogfile()
    {
        var path = Path.Combine(InitDir(), Logfile);
        if (!File.Exists(path)) return [];
        return await File.ReadAllLinesAsync(path);
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
        var path = Path.Combine(InitDir(), Logfile);
        return File.AppendAllLinesAsync(path, [line]);
    }

    public async Task CheckOwnerAsync()
    {
        var path = Path.Combine(InitDir(), OWNER_FILE);

        if (File.Exists(path))
        {
            var owner = (await File.ReadAllTextAsync(path)).Trim();
            if (owner != wf.Id)
                throw new ApplicationException(
                    $"Unerwarteter Zugriff auf Asset {SubDir} durch " +
                    $"Workflow {wf.Id}");
        }
        else
        {
            await File.WriteAllTextAsync(path, wf.Id);
        }
    }
}
