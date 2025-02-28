namespace WorkerDemo.Generic.WalzWorkflow;

public record WalzWorkflowMessage(
    string workflow_id,
    string key,
    object data);
