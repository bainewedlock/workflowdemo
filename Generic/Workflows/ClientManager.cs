using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.SignalR;

namespace WorkerDemo.Generic.Workflows
{
    public class ClientManager // : BackgroundService
    {
        ConcurrentDictionary<ISingleClientProxy, string> Clients = new();
        //static BufferBlock<WorkflowMessage> Queue = new();

        public async Task PublishAsync(WorkflowMessage m)
        {
            foreach (var c in Clients)
            {
                if (c.Value == m.workflow_id)
                {
                    await c.Key.SendAsync(m.key, m.data);
                }
            }
        }

        public void Join(ISingleClientProxy caller, string workflow_id)
        {
            Clients[caller] = workflow_id;
        }

        public void Disconnected(ISingleClientProxy caller)
        {
            Clients.Remove(caller, out var _);
        }
    }
}
