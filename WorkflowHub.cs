using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;

public class WorkflowHub : Hub
{

    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("action", "ähh");
        return base.OnConnectedAsync();
    }
    public async Task client_join(string workflow_id)
    {
        Debug.WriteLine($"client_join {workflow_id}");
        await Clients.All.SendAsync("action", "ähh");
        //await Clients.Caller.SendAsync("action", "ähh2");

        //await Clients.All.SendAsync("updates_message", new [] {
        //    new Dictionary<string, object>
        //    {
        //        { "name", "1" },
        //        { "target_value", "2" }
        //    }});
        await Clients.All.SendAsync("ReceiveMessage", "client_join", "message");
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

}