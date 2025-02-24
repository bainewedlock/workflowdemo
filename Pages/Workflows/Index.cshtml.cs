using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Model;

namespace WorkerDemo.Pages.Workflows
{
    public class IndexModel : PageModel
    {
        private readonly WorkflowContext db;

        public IndexModel(WorkflowContext db)
        {
            this.db = db;
        }

        public IList<Workflow> Workflow { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Workflow = await db.Workflows.ToListAsync();
        }

        public async Task<IActionResult> OnPostCleanup(string instanceId)
        {
            await db.Workflows
                .Where(x => x.Status == 2)
                .ExecuteDeleteAsync();
            return RedirectToPage();
        }
    }
}
