using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WorkflowEF;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Pages.Workflows
{
    public class IndexModel : PageModel
    {
        readonly IWorkflowHost wf;
        readonly WorkflowContext db;

        public IndexModel(IWorkflowHost wf, WorkflowContext db)
        {
            this.wf = wf;
            this.db = db;
        }

        public IList<WorkflowInstance> Workflows { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Workflows = (await wf.PersistenceStore.GetWorkflowInstances(
                status: null, type: null, createdFrom: null, createdTo: null,
                skip: 0, take: 100)).ToList();
        }

        public async Task<IActionResult> OnPostCleanup(string instanceId)
        {
            await db.Workflows
                .Where(x => x.Status == 2 || x.Status == 3) // complete/terminated
                .ExecuteDeleteAsync();
            return RedirectToPage();
        }
    }
}
