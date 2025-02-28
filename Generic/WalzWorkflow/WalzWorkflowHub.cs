using Microsoft.AspNetCore.SignalR;
using WorkflowCore.Interface;

namespace WorkerDemo.Generic.WalzWorkflow;

public class WalzWorkflowHub : Hub
{
    readonly IWorkflowHost wf;
    readonly ClientManager clients;
    readonly Assets.Factory assetsFactory;

    public WalzWorkflowHub(IWorkflowHost wf, ClientManager clients,
        Assets.Factory assetsFactory)
    {
        this.wf = wf;
        this.clients = clients;
        this.assetsFactory = assetsFactory;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Disconnected(Clients.Caller);
        return base.OnDisconnectedAsync(exception);
    }

    ////////////////////////////////////////////////////////////
    // SIGNALR CLIENT FUNCTIONS
    ////////////////////////////////////////////////////////////
    public void ClientJoin(string workflow_id)
    {
        clients.Join(Clients.Caller, workflow_id);
    }

    public void Resume(string workflow_id)
    {
        var a = assetsFactory(workflow_id);
        a.LogAsync(LogCategory.Workflow, $"user resume").Wait();
        // TODO: identify user
        wf.ResumeWorkflow(workflow_id);
    }
}