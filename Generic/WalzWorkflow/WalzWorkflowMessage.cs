namespace WorkerDemo.Generic.WalzWorkflow;

public record WalzWorkflowMessage(
    string workflow_id,
    string key,
    Dictionary<string, object> data);
