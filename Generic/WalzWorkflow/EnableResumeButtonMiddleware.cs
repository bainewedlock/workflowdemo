using WorkflowCore.Interface;
using WorkflowCore.Models;
namespace WorkerDemo.Generic.WalzWorkflow;

public class EnableResumeButtonMiddleware : IWorkflowMiddleware
{
    readonly ClientManager clients;

    public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.ExecuteWorkflow;

    public EnableResumeButtonMiddleware(ClientManager clients)
    {
        this.clients = clients;
    }

    public async Task HandleAsync(WorkflowInstance workflow, WorkflowDelegate next)
    {
        if (workflow.Status == WorkflowStatus.Suspended)
        {
            await clients.PublishAsync(new WalzWorkflowMessage(
                workflow_id: workflow.Id,
                key: "EnableResume",
                data: null!));
        }
        await next();
    }
}
