using WorkflowCore.Interface;
using WorkflowCore.Models;

public class LogMiddleware : IWorkflowStepMiddleware
{
    public async Task<ExecutionResult> HandleAsync(
        IStepExecutionContext context,
        IStepBody body, WorkflowStepDelegate next)
    {
        return await next();
    }
}
