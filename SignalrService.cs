using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.SignalR;

namespace WorkerDemo
{
    public class SignalrService : BackgroundService
    {
        static BufferBlock<string> Queue = new();
        readonly IHubContext<WorkflowHub> hubctx;

        public SignalrService(IHubContext<WorkflowHub> hubctx)
        {
            this.hubctx = hubctx;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var action = await Queue.ReceiveAsync(stoppingToken);
                Debug.Write($"sending action: {action}");
                //await hubctx.Clients.All.SendAsync("action", action);
                await hubctx.Clients.All.SendAsync("ReceiveMessage", "user " + Guid.NewGuid().ToString(), "message");
            }
        }

        public static void Enqueue(string action)
        {
            Queue.Post(action);
        }
    }
}
