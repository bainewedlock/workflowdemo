namespace WorkerDemo.SignalR
{
    public record WorkflowMessage(
        string workflow_id,
        string key,
        object data);
}
