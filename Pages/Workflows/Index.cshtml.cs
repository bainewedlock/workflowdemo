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

        public async Task<IActionResult> OnPostCleanup(string instanceId)
        {
            var date = DateTime.Today.AddDays(-14);
            await purger.PurgeWorkflows(WorkflowStatus.Terminated, date);
            await purger.PurgeWorkflows(WorkflowStatus.Complete, date);
            return RedirectToPage();
        }
    }
}
