using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WorkflowEF;

namespace WorkerDemo.Pages.Workflows
{
    public class EditModel : PageModel
    {
        public Workflow Workflow { get; set; } = default!;
        public ExecutionError[] Errors { get; set; } = { };
        readonly WorkflowContext db;

        public EditModel(WorkflowContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || await db.Workflows.FirstOrDefaultAsync(
                m => m.InstanceId == new Guid(id)) == null)
            {
                return NotFound();
            }
            else
            {
                Workflow = await db.Workflows
                .FirstOrDefaultAsync(m => m.InstanceId == new Guid(id));
                Errors = await db.ExecutionErrors
                    .Where(x => x.WorkflowId == id)
                    .ToArrayAsync();
            }
            return Page();
        }
    }
}
