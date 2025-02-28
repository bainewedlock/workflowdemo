namespace WorkerDemo.Generic.Workflows
{
    public record WorkflowMessage(
        string workflow_id,
        string key,
        object data);
}
