using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WorkflowEF;
using WorkflowCore.Interface;

namespace WorkerDemo.Pages.Workflows
{
    public class EditModel : PageModel
    {
        public Workflow Workflow { get; set; } = default!;
        public ExecutionError[] Errors { get; set; } = { };
        readonly WorkflowContext db;
        readonly IWorkflowHost wf;

        public EditModel(WorkflowContext db, 
            IWorkflowHost wf)
        {
            this.db = db;
            //this.wf = wf;
        }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflow = await db.Workflows
                .FirstOrDefaultAsync(m => m.InstanceId == new Guid(id));
            if (workflow == null)
            {
                return NotFound();
            }
            else
            {
                Workflow = workflow;
                Errors = await db.ExecutionErrors
                    .Where(x => x.WorkflowId == id)
                    .ToArrayAsync();
            }
            return Page();
        }

        //public async Task<IActionResult> OnPostResumeAsync(string persistenceId, string instanceId)
        //{
        //    bool ok = await wf.ResumeWorkflow(instanceId);
        //    return RedirectToPage("./Edit", new { id = instanceId });
        //}
    }
}
