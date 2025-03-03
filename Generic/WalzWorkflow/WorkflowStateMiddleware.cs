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

        // state of workflow is not yet in db at this point
        // so we set it manually
        msg.data["status"] = (int)workflow.Status;

        await clients.PublishAsync(msg);

        await next();
    }
}
