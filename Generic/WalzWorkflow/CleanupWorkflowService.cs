using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Generic.WalzWorkflow
{
    public class CleanupWorkflowService : BackgroundService
    {
        readonly ILogger<CleanupWorkflowService> log;
        readonly IWorkflowPurger purger;
        readonly TimeSpan rundelay = TimeSpan.FromDays(1);
        readonly TimeSpan purgedelay = TimeSpan.FromDays(14);

        public CleanupWorkflowService(ILogger<CleanupWorkflowService> log,
            IWorkflowPurger purger)
        {
            this.log = log;
            this.purger = purger;
        }

        protected override async Task ExecuteAsync(CancellationToken st)
        {
            using var timer = new PeriodicTimer(rundelay);
            do
            {
                try
                {
                    var d = DateTime.Now - purgedelay;
                    await purger.PurgeWorkflows(WorkflowStatus.Terminated, d);
                    await purger.PurgeWorkflows(WorkflowStatus.Complete, d);
                }
                catch (Exception ex)
                {
                    log.LogError(ex,
                        $"Failed to execute Cleanup. See you in {rundelay}!");
                }
            }
            while (!st.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(st));
        }
    }
}
