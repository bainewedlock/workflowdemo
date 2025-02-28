namespace WorkerDemo.Generic.WalzWorkflow;

public record WorkflowMessage(
    string workflow_id,
    string key,
    object data);
