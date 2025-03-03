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
        // state and completeTime of workflow is not yet in db at this point
        // so we have to set it manually
        var msg = await WalzWorkflowHub.GetWorkflowState(db, workflow.Id,
            workflow.Status, workflow.CompleteTime);

        await clients.PublishAsync(msg);

        await next();
    }
}
