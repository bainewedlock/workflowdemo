using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.SignalR;

namespace WorkerDemo.SignalR
{
    public class SignalrService : BackgroundService
    {
        static ConcurrentDictionary<ISingleClientProxy, string> Clients = new();
        static BufferBlock<WorkflowMessage> Queue = new();
        readonly IHubContext<WorkflowHub> hubctx;

        public SignalrService(IHubContext<WorkflowHub> hubctx)
        {
            this.hubctx = hubctx;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var m = await Queue.ReceiveAsync(stoppingToken);
                foreach (var c in Clients)
                {
                    if (c.Value == m.workflow)
                    {
                        await hubctx.Clients.All.SendAsync(
                            "ReceiveMessage", m.message, "x");
                    }
                }
            }
        }

        public static void Enqueue(WorkflowMessage m)
        {
            Queue.Post(m);
        }

        public static void Join(ISingleClientProxy caller, string workflow_id)
        {
            Clients[caller] = workflow_id;
        }

        public static void Disconnected(ISingleClientProxy caller)
        {
            Clients.Remove(caller, out var _);
        }
    }
}
