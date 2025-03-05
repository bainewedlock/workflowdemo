using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WorkflowEF;

namespace WorkerDemo.Pages.Workflows
{
    public class EditModel : PageModel
    {
        public Workflow Workflow { get; set; } = default!;
        readonly WorkflowContext db;

        public EditModel(WorkflowContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || await db.Workflows.FirstOrDefaultAsync(
                m => m.InstanceId == new Guid(id)) == null) return NotFound();

            Workflow = await db.Workflows.SingleAsync(
                m => m.InstanceId == new Guid(id));

            return Page();
        }
    }
}
