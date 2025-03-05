namespace WorkerDemo.Generic.WorkflowEF;

public partial class Workflow
{
    public string InstanceTitle
    {
        get
        {
            return  $"{this.WorkflowDefinitionId}/{this.Reference}";
        }
    }

}
