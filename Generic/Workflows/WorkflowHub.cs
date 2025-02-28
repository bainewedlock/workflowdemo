using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using WorkerDemo.Generic.Workflows;
using WorkflowCore.Interface;

public class WorkflowHub : Hub
{
    readonly IWorkflowHost wf;
    readonly ClientManager clients;

    public WorkflowHub(IWorkflowHost wf, ClientManager clients)
    {
        this.wf = wf;
        this.clients = clients;
    }

    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("action", "ähh");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Disconnected(Clients.Caller);
        return base.OnDisconnectedAsync(exception);
    }

    public void ClientJoin(string workflow_id)
    {
        clients.Join(Clients.Caller, workflow_id);
        Debug.WriteLine($"ClientJoin {workflow_id}");
    }

    public void Resume(string workflow_id)
    {
        wf.ResumeWorkflow(workflow_id);
    }
}