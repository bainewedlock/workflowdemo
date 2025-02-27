using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using WorkerDemo.SignalR;
using WorkflowCore.Interface;

public class WorkflowHub : Hub
{
    readonly IWorkflowHost wf;

    public WorkflowHub(IWorkflowHost wf)
    {
        this.wf = wf;
    }

    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("action", "ähh");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        SignalrService.Disconnected(Clients.Caller);
        return base.OnDisconnectedAsync(exception);
    }

    public void ClientJoin(string workflow_id)
    {
        SignalrService.Join(Clients.Caller, workflow_id);
        Debug.WriteLine($"ClientJoin {workflow_id}");
    }

    public void Resume(string workflow_id)
    {
        wf.ResumeWorkflow(workflow_id);
    }
}