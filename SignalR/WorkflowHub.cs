using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using WorkerDemo.SignalR;

public class WorkflowHub : Hub
{
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

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

}