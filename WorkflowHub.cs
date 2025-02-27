using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;

public class WorkflowHub : Hub
{
    public async Task client_join(string workflow_id)
    {
        Debug.WriteLine($"client_join {workflow_id}");
        await Clients.All.SendAsync("action", "ähh");

    }
}
// fehler: https://localhost:7203/js/signalr/dist/browser/signalr.js