using Microsoft.AspNetCore.SignalR;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Generic.WalzWorkflow;

public class WalzWorkflowHub : Hub
{
    readonly IWorkflowHost wf;
    readonly ClientManager clients;
    readonly Assets.Factory assets;

    public WalzWorkflowHub(IWorkflowHost wf, ClientManager clients,
        Assets.Factory assets)
    {
        this.wf = wf;
        this.clients = clients;
        this.assets = assets;
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

        var wfi = await wf.PersistenceStore.GetWorkflowInstance(workflow_id);
        var m = GetWorkflowState(wfi);

        ///////////////////////////////////////////////////////////////////////
        // add logs from textfile
        ///////////////////////////////////////////////////////////////////////
        var log = new List<LogEntry>();
        var a = assets(wfi);
        foreach (var l in await a.ReadLogfile())
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

    public static WalzWorkflowMessage GetWorkflowState(WorkflowInstance wfi)
    {
        var data = new Dictionary<string, object>
        {
            ["status"] = (int)wfi.Status,
            ["complete_time"] = wfi.CompleteTime != null ?
                                wfi.CompleteTime.ToString()! : "-",
            ["can_resume"] = wfi.Status == WorkflowStatus.Suspended
        };

        return new WalzWorkflowMessage
        (
            workflow_id: wfi.Id,
            key: "WorkflowState",
            data: data
        );
    }

    public async Task Resume(string workflow_id)
    {
        var wfi = await wf.PersistenceStore.GetWorkflowInstance(workflow_id);
        var a = assets(wfi);
        var user = Context.User!.Identity!;
        await a.LogAsync(LogCategory.Workflow, $"resumed by {user.Name}");
        await wf.ResumeWorkflow(workflow_id);
    }

    public async Task Terminate(string workflow_id)
    {
        var wfi = await wf.PersistenceStore.GetWorkflowInstance(workflow_id);
        var a = assets(wfi);
        var user = Context.User!.Identity!;
        await a.LogAsync(LogCategory.Workflow, $"terminated by {user.Name}");
        await wf.TerminateWorkflow(workflow_id);
    }
}