using WorkflowCore.Interface;
using WorkflowCore.Models;
namespace WorkerDemo.Generic.WalzWorkflow;

public class WorkflowStateMiddleware : IWorkflowMiddleware
{
    readonly ClientManager clients;

    public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.ExecuteWorkflow;

    public WorkflowStateMiddleware(ClientManager clients)
    {
        this.clients = clients;
    }

    public async Task HandleAsync(WorkflowInstance workflow, WorkflowDelegate next)
    {
        // state and completeTime of workflow is not yet in db at this point
        // so we have to set it manually
        var msg = WalzWorkflowHub.GetWorkflowState(workflow);

        await clients.PublishAsync(msg);

        await next();
    }
}
