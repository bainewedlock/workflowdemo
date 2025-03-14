using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Pages.Workflows
{
    public class IndexModel : PageModel
    {
        readonly IWorkflowHost host;
        readonly IWorkflowPurger purger;

        public IndexModel(IWorkflowHost host, IWorkflowPurger purger)
        {
            this.host = host;
            this.purger = purger;
        }

        public IList<WorkflowInstance> Workflows { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Workflows = (await host.PersistenceStore.GetWorkflowInstances(
                status: null, type: null, createdFrom: null, createdTo: null,
                skip: 0, take: 100)).ToList();
        }
        public async Task<IActionResult> OnPostStart(string instanceId)
        {
            var rnd = 12345;
            await host.StartWorkflow("Demo", null, $"61.{rnd}.1.2");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCleanup(string instanceId)
        {
            await Purge(DateTime.Now);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPurge(string instanceId)
        {
            await Purge(DateTime.Today.AddDays(-14));
            return RedirectToPage();
        }

        async Task Purge(DateTime date)
        {
            await purger.PurgeWorkflows(WorkflowStatus.Terminated, date);
            await purger.PurgeWorkflows(WorkflowStatus.Complete, date);
        }
    }
}
