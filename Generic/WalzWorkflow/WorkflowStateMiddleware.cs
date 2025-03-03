using WorkerDemo.Generic.WorkflowEF;
using WorkflowCore.Interface;
using WorkflowCore.Models;
namespace WorkerDemo.Generic.WalzWorkflow;

public class WorkflowStateMiddleware : IWorkflowMiddleware
{
    readonly ClientManager clients;
    readonly WorkflowContext db;

    public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.ExecuteWorkflow;

    public WorkflowStateMiddleware(ClientManager clients, WorkflowContext db)
    {
        this.clients = clients;
        this.db = db;
    }

    public async Task HandleAsync(WorkflowInstance workflow, WorkflowDelegate next)
    {
        var msg = await WalzWorkflowHub.GetWorkflowState(db, workflow.Id);

        // state of suspended workflows is not yet in db at this point
        // so we set it manually
        if (workflow.Status == WorkflowStatus.Suspended)
        {
            msg.data["status"] = 1;
        }

        await clients.PublishAsync(msg);

        await next();
    }
}
