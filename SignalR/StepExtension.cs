using WorkflowCore.Interface;

namespace WorkerDemo.SignalR
{
    public static class StepExtension
    {
        public static void LogSignal(this IStepExecutionContext ctx,
            string message)
        {
            var blub = new Dictionary<string, object>
            {
                { "timestamp", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") },
                { "message", message }
            };

            SignalrService.Enqueue(new WorkflowMessage(
                workflow_id: ctx.Workflow.Id,
                key: "Log",
                data: blub));
        }
    }
}
