using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using WorkerDemo.Generic.WorkflowEF;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Generic.WalzWorkflow;

public class WalzWorkflowHub : Hub
{
    readonly IWorkflowHost wf;
    readonly ClientManager clients;
    readonly Assets.Factory assets;
    readonly WorkflowContext db;

    public WalzWorkflowHub(IWorkflowHost wf, ClientManager clients,
        Assets.Factory assets, WorkflowContext db)
    {
        this.wf = wf;
        this.clients = clients;
        this.assets = assets;
        this.db = db;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Disconnected(Clients.Caller);
        return base.OnDisconnectedAsync(exception);
    }

    ////////////////////////////////////////////////////////////
    // SIGNALR CLIENT FUNCTIONS
    ////////////////////////////////////////////////////////////
    public async Task ClientJoin(string workflow_id)
    {
        clients.Join(Clients.Caller, workflow_id);

        var m = await GetWorkflowState(db, workflow_id);

        ///////////////////////////////////////////////////////////////////////
        // add logs from textfile
        ///////////////////////////////////////////////////////////////////////
        var log = new List<LogEntry>();
        var a = assets(workflow_id);
        foreach (var l in await a.ReadStringLinesAsync("logfile.txt"))
        {
            var tokens = l.Split();
            var left = string.Join(" ", tokens.Take(2));
            var ctx = tokens[2].Replace("[", "").Replace("]", "");
            var right = string.Join(" ", tokens.Skip(3));

            log.Add(new LogEntry ( time: left, context: ctx, message: right ));
        }
        m.data.Add("log", log);
        ///////////////////////////////////////////////////////////////////////

        await Clients.Caller.SendAsync(m.key, m.data);
    }

    public static async Task<WalzWorkflowMessage> GetWorkflowState(
        WorkflowContext db, string workflow_id,
        WorkflowStatus? overrideStatus = null,
        DateTime? overrideCompleteTime = null)
    {
        var wf = await db.Workflows.SingleAsync(
            x => x.InstanceId == new Guid(workflow_id));

        var status = overrideStatus ?? (WorkflowStatus)wf.Status;

        var complete_time = overrideCompleteTime ?? wf.CompleteTime;

        var data = new Dictionary<string, object>
        {
            ["status"] = (int)status,
            ["complete_time"] = complete_time != null ?
                                complete_time.ToString()! : "-",
            ["can_resume"] = status == WorkflowStatus.Suspended
        };

        return new WalzWorkflowMessage
        (
            workflow_id: workflow_id,
            key: "WorkflowState",
            data: data
        );
    }

    public async Task Resume(string workflow_id)
    {
        var a = assets(workflow_id);
        await a.LogAsync(LogCategory.Workflow, $"user resume");
        // TODO: identify user
        await wf.ResumeWorkflow(workflow_id);
    }
}